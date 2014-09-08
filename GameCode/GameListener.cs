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
        public bool Running { get; set; }

        public GameListener(NetworkClient netClient, GameManager manager)
        {
            Console.WriteLine("{0} GameListener - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            NetClient = netClient;
            Manager = manager;
            Running = false;
        }

        /// <summary>
        /// The thread code
        /// </summary>
        public void Start()
        {
            Running = true;
            while (Running)
            {
                string line = "";
                try
                {
                    // read the message from the server
                    line = NetClient.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] data = line.Split(',');
                        int connectionID = int.Parse(data[0]);
                        int messageType = int.Parse(data[1]);
                        int objectType = int.Parse(data[2]);
                        int objectID = int.Parse(data[3]);

                        int amount;
                        double ang;
                        Vector3 pos, vel;

                        // What does the message say?
                        switch (messageType)
                        {
                            case GameConstants.MSG_ADD:

                                messageType = int.Parse(data[1]);
                                objectType = int.Parse(data[2]);
                                pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
                                vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
                                ang = double.Parse(data[10]);

                                Manager.AddFromListener(objectID, messageType, objectType, pos, vel, ang, data, false);
                                break;

                            case GameConstants.MSG_DEAD:

                                Manager.RemoveObject(objectID, false);
                                break;

                            case GameConstants.MSG_DECREASE_GOLD:

                                amount = int.Parse(data[4]);
                                Manager.DecreaseGold(objectID, amount, false);
                                break;

                            case GameConstants.MSG_DECREASE_HP:
                                
                                amount = int.Parse(data[4]);
                                Manager.DecreaseHealth(objectID, amount, false);
                                break;

                            case GameConstants.MSG_GAMEOVER:
                                
                                Manager.EndGame(false);
                                break;

                            case GameConstants.MSG_INCREASE_HP:

                                amount = int.Parse(data[4]);
                                Manager.IncreaseHealth(objectID, amount, false);
                                break;

                            case GameConstants.MSG_INCREASE_STAT:

                                int statType = int.Parse(data[2]);
                                amount = int.Parse(data[4]);
                                Manager.IncreaseStat(objectID, statType, amount, false);
                                break;

                            case GameConstants.MSG_LEVEL_UP:

                                Manager.LevelUpCharacter(objectID, false);
                                break;

                            case GameConstants.MSG_REQUEST_ALL_DATA:

                                Manager.SendAllObjects();
                                break;

                            //case GameConstants.MSG_STOP_LISTENING:
                            //    ListenerRemove(objectID, data);
                            //    break;

                            case GameConstants.MSG_UPDATE:

                                if (Manager.GetCurrentCharacter().ID != objectID) // if the character is from a different manager
                                {
                                    messageType = int.Parse(data[1]);
                                    objectType = int.Parse(data[2]);
                                    pos = new Vector3(double.Parse(data[4]), double.Parse(data[5]), double.Parse(data[6]));
                                    vel = new Vector3(double.Parse(data[7]), double.Parse(data[8]), double.Parse(data[9]));
                                    ang = double.Parse(data[10]);

                                    Manager.UpdateFromListener(objectID, messageType, objectType, pos, vel, ang, data);
                                }
                                break;

                            case GameConstants.MOVEMENT_ATTACK:
                                int ownerID = int.Parse(data[11]);
                                Manager.SubmitBotAttack(ownerID);
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
    }
}
