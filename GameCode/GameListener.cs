using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode
{
    public class GameListener
    {
        private NetworkClient NetClient;
        private GameManager Manager;
        private GameWorld World;
        public bool Running { get; set; }

        public GameListener(NetworkClient netClient, GameManager manager)
        {
            Console.WriteLine("{0} GameListener - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            NetClient = netClient;
            Manager = manager;
            World = Manager.World;
            Running = false;
        }

        public void Start()
        {
            Running = true;
            Console.WriteLine("{0} GameListener - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            while (Running)
            {
                //Console.WriteLine("{0} GameListener - Receiving", System.Threading.Thread.CurrentThread.ManagedThreadId);
                string line = "";
                try
                {
                    line = NetClient.ReadLine();
                    //Console.WriteLine("{0} GameListener - Receiving: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, line);
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] data = line.Split(',');
                        int connectionID = int.Parse(data[0]);
                        int messageType = int.Parse(data[1]);
                        //int objectType = int.Parse(data[2]);
                        int objectID = int.Parse(data[3]);

                        //Console.WriteLine("messageType: {0}, objID: {1}, line: {2}:", messageType, objectID, line);

                        // Listen for client stuff
                        switch (messageType)
                        {
                            case GameConstants.MSG_ADD:
                                Add(objectID, data);
                                break;
                            case GameConstants.MSG_DEAD:
                                Remove(objectID, data);
                                break;
                            case GameConstants.MSG_DECREASE_HP:
                                DecreaseHealth(objectID, data);
                                break;
                            case GameConstants.MSG_GAMEOVER:
                                Manager.EndGame();
                                Running = false;
                                break;
                            case GameConstants.MSG_INCREASE_HP:
                                IncreaseHealth(objectID, data);
                                break;
                            case GameConstants.MSG_INCREASE_STAT:
                                IncreaseStat(objectID, data);
                                break;
                            case GameConstants.MSG_REQUEST_ALL_DATA:
                                // send every object
                                foreach (GameObject o in World.Objects)
                                {
                                    if (o.Alive)
                                    {
                                        //Manager.SendInfo(msgString);
                                        Manager.SendInfo(MessageBuilder.AddMessage(o));
                                    }
                                }
                                break;
                            case GameConstants.MSG_STOP_LISTENING:
                                break;
                            case GameConstants.MSG_UPDATE:
                                Update(objectID, data);
                                break;
                            case GameConstants.MOVEMENT_ATTACK:
                                int ownerID = int.Parse(data[11]);
                                ((Character)World.Get(ownerID)).Weapon.Attack();
                                break;
                            default:
                                throw new ArgumentException(string.Format("Received bad input: {0}", messageType));
                                break;
                        }
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Bad format for: {0}", line);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error listening: {0}\n{1}", line, ex.Message);
                }
            }
        }

        private void Add(int objectID, string[] data)
        {
            //Console.WriteLine("{0} GameListener - Add: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
            int messageType = int.Parse(data[1]);
            int objectType = int.Parse(data[2]);
            //Console.WriteLine("{0} GameListener - ObjType: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectType);
            Vector3 pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
            Vector3 vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
            double ang = double.Parse(data[10]);

            GameObject o = null;

            if (objectType > GameConstants.TYPE_BOT_LOW && objectType < GameConstants.TYPE_BOT_HIGH) // its a bot
            {
                o = new Bot(pos, Manager, objectType)
                {
                    Angle = ang,
                    Velocity = vel,
                    ID = objectID
                };
            }
            else if (objectType > GameConstants.TYPE_CHARACTER_LOW && objectType < GameConstants.TYPE_CHARACTER_HIGH) // its a character
            {
                //int damage = int.Parse(data[11]);
                o = new Character(pos, Manager, null, objectType)
                {
                    Angle = ang,
                    Velocity = vel,
                    ID = objectID
                };
            }
            else if (objectType > GameConstants.TYPE_DEBRIS_LOW && objectType < GameConstants.TYPE_DEBRIS_HIGH) // its a debris
            {
                switch (objectType)
                {
                    case GameConstants.TYPE_DEBRIS_BUSH:
                        o = new Bushes(pos, Manager, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_DEBRIS_ROCK:
                        o = new Rocks(pos, Manager, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_DEBRIS_WALL:
                        o = new CastleWalls(pos, Manager, vel) // for Debris, velocity is the size
                        {
                            Position = pos,
                            ID = objectID
                        };
                        break;
                }
            }
            else if (objectType > GameConstants.TYPE_PROJ_LOW && objectType < GameConstants.TYPE_PROJ_HIGH) // its a projectile
            {
                int ownerID = int.MaxValue;
                if (messageType == GameConstants.MOVEMENT_ATTACK)
                {
                    ownerID = int.Parse(data[11]);
                }
                switch (objectType)
                {
                    case GameConstants.TYPE_PROJ_ARROW:
                        o = new Arrow(ownerID, Manager, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_PROJ_FIRE:
                        o = new FireBall(ownerID, Manager, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                    case GameConstants.TYPE_PROJ_STAB:
                        o = new StabAttack(ownerID, Manager, ang)
                        {
                            Position = pos,
                            Velocity = vel,
                            ID = objectID
                        };
                        break;
                }
            }
            Manager.AddObjectThreadSafe(o);
        }

        private void Remove(int objectID, string[] data)
        {
            //Console.WriteLine("{0} GameListener - Remove: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
            Manager.RemoveObject(objectID);
        }

        private void Update(int objectID, string[] data)
        {
            if (Manager.GetCurrentCharacter().ID != objectID)
            {
                //Console.WriteLine("{0} GameListener - Update: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
                Vector3 pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
                Vector3 vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
                double ang = double.Parse(data[10]);

                GameObject o = World.Get(objectID);
                if (o != null)
                {
                    o.Angle = ang;
                    o.Position = pos;
                }
                else
                {
                    Add(objectID, data);
                }
            }
        }

        private void DecreaseHealth(int objectID, string[] data)
        {
            if (Manager.GetCurrentCharacter().ID != objectID)
            {
                //Console.WriteLine("{0} GameListener - IncreaseStat: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
                //int statType = int.Parse(data[2]);
                int amount = int.Parse(data[4]);

                Bot o = (Bot)World.Get(objectID);
                if (o != null)
                {
                    o.Health -= amount;
                }
                else
                {
                    //                    Add(objectID, data);
                }
            }
        }

        private void IncreaseHealth(int objectID, string[] data)
        {
            if (Manager.GetCurrentCharacter().ID != objectID)
            {
                //Console.WriteLine("{0} GameListener - IncreaseStat: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
                //int statType = int.Parse(data[2]);
                int amount = int.Parse(data[4]);

                Bot o = (Bot)World.Get(objectID);
                if (o != null)
                {
                    o.Health += amount;
                }
                else
                {
                    //                    Add(objectID, data);
                }
            }
        }

        private void IncreaseStat(int objectID, string[] data)
        {
            if (Manager.GetCurrentCharacter().ID != objectID)
            {
                //Console.WriteLine("{0} GameListener - IncreaseStat: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, objectID);
                int statType = int.Parse(data[2]);
                int amount = int.Parse(data[4]);

                Character o = (Character)World.Get(objectID);
                if (o != null)
                {
                    switch (statType)
                    {
                        case GameConstants.STAT_XP:
                            o.Experience += amount;
                            break;
                    }
                }
                else
                {
                    //                    Add(objectID, data);
                }
            }
        }


    }
}
