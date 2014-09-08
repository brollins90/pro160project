using System;
using GameCode.Helpers;
using GameCode.Models;
using GameCode.Models.Projectiles;

namespace GameCode
{
    /// <summary>
    /// The code that will listen on the network for updates
    /// </summary>
    public class GameListener
    {
        private NetworkClient NetClient;
        private GameManager Manager;
        //private GameWorld World;
        public bool Running { get; set; }

        public GameListener(NetworkClient netClient, GameManager manager)
        {
            Console.WriteLine("{0} GameListener - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            NetClient = netClient;
            Manager = manager;
            //World = Manager.World;
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
                                ListenerAdd(objectID, data);
                                break;
                            case GameConstants.MSG_DEAD:
                                ListenerRemove(objectID, data);
                                break;
                            case GameConstants.MSG_DECREASE_GOLD:
                                ListenerDecreaseGold(objectID, data);
                                break;
                            case GameConstants.MSG_DECREASE_HP:
                                ListenerDecreaseHealth(objectID, data);
                                break;
                            case GameConstants.MSG_GAMEOVER:
                                ListenerEndGame(objectID, data);
                                break;
                            case GameConstants.MSG_INCREASE_HP:
                                ListenerIncreaseHealth(objectID, data);
                                break;
                            case GameConstants.MSG_INCREASE_STAT:
                                ListenerIncreaseStat(objectID, data);
                                break;
                            case GameConstants.MSG_LEVEL_UP:
                                ListenerLevelUp(objectID, data);
                                break;
                            case GameConstants.MSG_REQUEST_ALL_DATA:
                                ListenerSendAllObjects(objectID, data);
                                break;
                            //case GameConstants.MSG_STOP_LISTENING:
                            //    ListenerRemove(objectID, data);
                            //    break;
                            case GameConstants.MSG_UPDATE:
                                ListenerUpdate(objectID, data);
                                break;
                            case GameConstants.MOVEMENT_ATTACK:
                                ListenerAttack(objectID, data);
                                break;
                            default:
                                throw new ArgumentException(string.Format("Received bad input: {0}", messageType));
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

        private void ListenerAdd(int objectID, string[] data)
        {
            int messageType = int.Parse(data[1]);
            int objectType = int.Parse(data[2]);

            Vector3 pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
            Vector3 vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
            double ang = double.Parse(data[10]);

            Manager.AddFromListener(objectID, messageType, objectType, pos, vel, ang, data, false);
        }

        private void ListenerAttack(int objectID, string[] data)
        {
            int ownerID = int.Parse(data[11]);
            Manager.SubmitBotAttack(ownerID);
        }

        private void ListenerDecreaseGold(int objectID, string[] data)
        {
            //if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
            //{
                int amount = int.Parse(data[4]);
                Manager.DecreaseGold(objectID, amount, false);
            //}
        }

        private void ListenerDecreaseHealth(int objectID, string[] data) // if the character is from a different manager
        {
            //if (Manager.GetCurrentCharacter().ID != objectID)
            //{
                int amount = int.Parse(data[4]);

                Manager.DecreaseHealth(objectID, amount, false);
            //}
        }

        private void ListenerEndGame(int objectID, string[] data) // if the character is from a different manager
        {
            //if (Manager.GetCurrentCharacter().ID != objectID)
            //{
                Manager.EndGame(false);
            //}
        }

        private void ListenerIncreaseHealth(int objectID, string[] data) // if the character is from a different manager
        {
            //if (Manager.GetCurrentCharacter().ID != objectID)
            //{
                int amount = int.Parse(data[4]);

                Manager.IncreaseHealth(objectID, amount, false);
            //}
        }

        private void ListenerIncreaseStat(int objectID, string[] data)
        {
            //if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
            //{
                int statType = int.Parse(data[2]);
                int amount = int.Parse(data[4]);

                Manager.IncreaseStat(objectID, statType, amount, false);
            //}
        }

        private void ListenerLevelUp(int objectID, string[] data)
        {
            //if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
            //{
                Manager.LevelUpCharacter(objectID, false);
            //}
        }

        private void ListenerRemove(int objectID, string[] data)
        {
            Manager.RemoveObject(objectID, false);
        }

        private void ListenerSendAllObjects(int objectID, string[] data)
        {
            //if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
            //{
                Manager.SendAllObjects();
            //}
        }

        private void ListenerUpdate(int objectID, string[] data)
        {
            if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
            {
                int messageType = int.Parse(data[1]);
                int objectType = int.Parse(data[2]);

                Vector3 pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
                Vector3 vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
                double ang = double.Parse(data[10]);

                Manager.UpdateFromListener(objectID, messageType, objectType, pos, vel, ang, data);
            }
        }
    }
}
