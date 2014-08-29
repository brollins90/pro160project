using GameCode.Helpers;
using GameCode.Models;
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

        public GameListener(NetworkClient netClient, GameManager manager)
        {
            //Console.WriteLine("{0} GameListener - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            NetClient = netClient;
            Manager = manager;
            World = Manager.World;
        }

        public void Start()
        {
            Console.WriteLine("{0} GameListener - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
                //Console.WriteLine("{0} GameListener - Receiving", System.Threading.Thread.CurrentThread.ManagedThreadId);
                try
                {
                    string line = NetClient.ReadLine();
                    string[] data = line.Split(',');
                    int connectionID = int.Parse(data[0]);
                    int messageType = int.Parse(data[1]);
                    int objectType = int.Parse(data[2]);
                    int objectID = int.Parse(data[3]);

                    //Console.WriteLine("messageType: {0}, objID: {1}, line: {2}:", messageType, objectID, line);

                    // Listen for client stuff
                    if (messageType == GameConstants.MSG_ADD)
                    {
                        Add(objectID, data);
                    }
                    else if (messageType == GameConstants.MSG_UPDATE)
                    {
                        Update(objectID, data);
                    }
                    else if (messageType == GameConstants.MSG_DEAD)
                    {
                        Remove(objectID, data);
                    }
                    //else if (messageType == GameConstants.MSG_GAMEOVER)
                    //{
                    //    Manager.EndGame();
                    //}
                    //else
                    //{

                    //}


                    // Listen for server stuff
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Bad format for: {0}", ex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error listening: {0}",ex.ToString());
                }
            }
        }

        private void Add(int objectID, string[] data)
        {
            throw new NotImplementedException();
        }

        private void Remove(int objectID, string[] data)
        {
            throw new NotImplementedException();
        }

        private void Update(int objectID, string[] data)
        {
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
}
