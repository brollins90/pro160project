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

        public ClientWorker(TcpClient client, List<ClientWorker> allClientWorkers, int conn)
        {
            Client = client;
            AllClientWorkers = allClientWorkers;
            Connection = conn;
        }
        internal void Start()
        {
            String line;
            try
            {
                //ClientStream = Client.GetStream();
                sr = new StreamReader(Client.GetStream());
                sw = new StreamWriter(Client.GetStream());
            }
            catch (IOException e)
            {
                Console.WriteLine("in or out failed");
                return;
            }

            while (true)
            {
                try
                {
                    line = sr.ReadLine();
                    //Send data back to other clients
                    //synchronized (AllClientWorkers) {
                    foreach (ClientWorker cw in AllClientWorkers)
                    {
                        if (cw != this)
                        {
                            cw.sw.WriteLine(Connection + "," + line);
                        }
                    }
                    //}
                }
                catch (IOException e)
                {
                    //remove the failed client
                    Console.WriteLine("Fuck you guys.");
                    //synchronized (AllClientWorkers) {
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
                    //}
                }
            }
        }
    }
}
