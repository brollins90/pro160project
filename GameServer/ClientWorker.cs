using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// Code to manage a client for the game
    /// </summary>
    public class ClientWorker
    {
        private TcpClient Client;
        private List<ClientWorker> AllClientWorkers;
        private StreamReader sr;
        private StreamWriter sw;
        private int Connection;
        public bool Running { get; set; }
        private int ErrorCount;

        public ClientWorker(TcpClient client, List<ClientWorker> allClientWorkers, int conn)
        {
            Client = client;
            AllClientWorkers = allClientWorkers;
            Connection = conn;
            Running = false;
            ErrorCount = 0;
        }

        /// <summary>
        /// Start the client thread
        /// </summary>
        internal void Start()
        {
            Running = true;
            String line = "";
            try
            {
                sr = new StreamReader(Client.GetStream());
                sw = new StreamWriter(Client.GetStream());
            }
            catch (IOException ex)
            {
                Console.WriteLine("Failed creating the network streams: {0}", ex.ToString());
                return;
            }

            while (Running)
            {
                try
                {
                    // read a line
                    line = sr.ReadLine();

                    //Send data back to other clients
                    lock (AllClientWorkers)
                    {
                        foreach (ClientWorker cw in AllClientWorkers)
                        {
                            // if this client is not itself
                            if (cw != this)
                            {
                                try
                                {
                                    // write the message to the other clients
                                    cw.sw.WriteLine(Connection + "," + line);
                                    cw.sw.Flush();
                                }
                                catch (Exception)
                                {
                                    // even if there is a temporary error, dont worry yet
                                    ErrorCount++;
                                    if (ErrorCount > 3)
                                    {
                                        throw new IOException();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    //remove the failed client
                    Console.WriteLine("The client has failed: {0}", ex.Message);
                    Console.WriteLine("last line was: {0}", line);
                    lock (AllClientWorkers)
                    {
                        int index = AllClientWorkers.FindIndex(x => x == this);
                        //if index == 0, remove the entire room (lost game server)
                        if (index == 0)
                        {
                            foreach (ClientWorker cw in AllClientWorkers)
                            {
                                if (cw != this)
                                {
                                    cw.sw.WriteLine(Connection + "," + GameCode.GameConstants.MSG_GAMEOVER);
                                    cw.sw.Flush();
                                }
                            }
                            AllClientWorkers.Clear();

                        }
                        else
                        {
                            AllClientWorkers.RemoveAt(index);
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Stop the thread
        /// </summary>
        public void Stop()
        {
            sr.Close();
            sw.Close();
            Running = false;
        }
    }
}
