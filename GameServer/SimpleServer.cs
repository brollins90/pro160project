using GameCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    public class SimpleServer
    {
        private TcpListener tcpListener;
        private List<ClientWorker> AllClientWorkers = new List<ClientWorker>();
        private int Connections = 1337;
        public static int ServerPort = 3333;
        public bool Running { get; set; }

        public SimpleServer()
        {
            Console.WriteLine("{0} SimpleServer - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Running = false;
            //Console.WriteLine("Server running");
        }

        public void Start()
        {
            Console.WriteLine("{0} SimpleServer - Start", System.Threading.Thread.CurrentThread.ManagedThreadId);
            Running = true;
            bool started = false;
            while (Running)
            {
                try
                {
                    if (!started)
                    {
                        tcpListener = new TcpListener(IPAddress.Any, ServerPort);
                        tcpListener.Start();
                        started = true;
                    }
                    if (!tcpListener.Pending())
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    TcpClient c = tcpListener.AcceptTcpClient();
                    StreamReader sr = new StreamReader(c.GetStream());
                    StreamWriter sw = new StreamWriter(c.GetStream());
                    sw.WriteLine("Welcome to the game server.\n");
                    sw.Flush();
                    sw.WriteLine("Now hosting " + (AllClientWorkers.Count + 1) + " clients.\n");
                    sw.Flush();
                    sw.WriteLine((AllClientWorkers.Count == 0) ? "!is server\n" : "!not server\n");
                    sw.Flush();
                    ClientWorker w = new ClientWorker(c, AllClientWorkers, Connections);
                    Connections++;
                    AllClientWorkers.Add(w);
                    new Thread(w.Start).Start();
                }
                catch (SocketException)
                {
                    Console.WriteLine("cannot bind to socket, already in use.");// {0}", ex.ToString());
                    break;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Accept failed: {0}", ex.ToString());
                    
                    Running = false;
                    Stop();
                    break;
                }
                
            }            
        }

        public void Stop()
        {
            try
            {
                foreach (ClientWorker cw in AllClientWorkers)
                {
                    cw.Stop();
                }
                tcpListener.Stop();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Failed to Stop tcplistener: {0}", ex.Message);
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                SimpleServer simple = new SimpleServer();
                //new Thread(simple.Start).Start();
                simple.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not listen on server port: {0}", ex.ToString());
            }
        }
    }
}
