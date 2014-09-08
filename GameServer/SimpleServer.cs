using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameCode;

namespace GameServer
{
    /// <summary>
    /// The Server code
    /// </summary>
    public class SimpleServer
    {
        private TcpListener tcpListener;
        private List<ClientWorker> AllClientWorkers = new List<ClientWorker>();
        private int Connections = 1337;
        public static int ServerPort = 3333;
        public bool Running { get; set; }

        public SimpleServer()
        {
            Running = false;
        }

        public void Start()
        {
            Running = true;
            bool started = false;
            while (Running)
            {
                try
                {
                    // Start the TcpListener
                    if (!started)
                    {
                        tcpListener = new TcpListener(IPAddress.Any, ServerPort);
                        tcpListener.Start();
                        started = true;
                    }
                    // Avoid always being blocked by an accept request
                    if (!tcpListener.Pending())
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    // accept a new client
                    TcpClient c = tcpListener.AcceptTcpClient();
                    StreamReader sr = new StreamReader(c.GetStream());
                    StreamWriter sw = new StreamWriter(c.GetStream());
                    sw.WriteLine("Welcome to the game server.\n");
                    sw.Flush();
                    sw.WriteLine("Now hosting " + (AllClientWorkers.Count + 1) + " clients.\n");
                    sw.Flush();
                    sw.WriteLine((AllClientWorkers.Count == 0) ? "!is server\n" : "!not server\n");
                    sw.Flush();
                    // Create a new client thread
                    ClientWorker w = new ClientWorker(c, AllClientWorkers, Connections);
                    Connections++;
                    AllClientWorkers.Add(w);
                    // start the client thread
                    new Thread(w.Start).Start();
                }
                catch (SocketException)
                {
                    Console.WriteLine("cannot bind to socket, already in use.");
                    break;
                }
                catch (IOException ex)
                {
                    // If we get an IO Exception on this level, stop the server
                    Console.WriteLine("Accept failed: {0}", ex.ToString());
                    
                    Running = false;
                    Stop();
                    break;
                }
                
            }            
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            try
            {
                // Stop each client
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

        /// <summary>
        /// Run the server
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                SimpleServer simple = new SimpleServer();
                simple.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not listen on server port: {0}", ex.ToString());
            }
        }
    }
}
