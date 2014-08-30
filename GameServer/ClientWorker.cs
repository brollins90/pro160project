using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class ClientWorker
    {
        private TcpClient Client;
        private List<ClientWorker> AllClientWorkers;
        //NetworkStream ClientStream;
        private StreamReader sr;
        private StreamWriter sw;
        private int Connection;
        public bool Running { get; set; }
        private int ErrorCount;

        public ClientWorker(TcpClient client, List<ClientWorker> allClientWorkers, int conn)
        {
            Console.WriteLine("{0} ClientWorker - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Client = client;
            AllClientWorkers = allClientWorkers;
            Connection = conn;
            Running = false;
            ErrorCount = 0;
        }
        internal void Start()
        {
            Console.WriteLine("{0} ClientWorker - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Running = true;
            String line = "";
            try
            {
                //ClientStream = Client.GetStream();
                sr = new StreamReader(Client.GetStream());
                sw = new StreamWriter(Client.GetStream());
            }
            catch (IOException ex)
            {
                Console.WriteLine("in or out failed: {0}", ex.ToString());
                return;
            }

            while (Running)
            {
                try
                {
                    line = sr.ReadLine();
                    //Console.WriteLine("{0} ClientWorker - Read: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, line);
                    //Send data back to other clients
                    lock (AllClientWorkers) {
                    foreach (ClientWorker cw in AllClientWorkers)
                        {
                            if (cw != this) // Server should not update itself...
                            {
                                try
                                {
                                    cw.sw.WriteLine(Connection + "," + line);
                                }
                                catch (Exception)
                                {
                                    // even if there is a temporary error, dont worry yet
                                    ErrorCount++;
                                    if (ErrorCount > 3)
                                    {
                                        throw;
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
                    lock (AllClientWorkers) {
                        int index = AllClientWorkers.FindIndex(x => x == this);
                        //if index == 0, remove the entire room (lost game server)
                        if (index == 0)
                        {
                            foreach (ClientWorker cw in AllClientWorkers)
                            {
                                if (cw != this)
                                {
                                    cw.sw.WriteLine(Connection + ",e");
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
        public void Stop()
        {
            sr.Close();
            sw.Close();
            Running = false;
        }
    }
}
