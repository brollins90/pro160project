using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class MultiServer
    {

        private TcpListener tcpListener;
        private List<MultiServerRoom> rooms = new List<MultiServerRoom>();
        private int Connections = 1337;
        public static int ServerPort = 3333;

        public MultiServer()
        {
            tcpListener = new TcpListener(IPAddress.Any, ServerPort);
            Console.WriteLine("Server running");
        }

        public void Start()
        {
            tcpListener.Start();
            while (true)
            {
                try
                {
                    MultiServerClientSetup clientSetupAgent = new MultiServerClientSetup(tcpListener.AcceptTcpClient(), rooms, Connections);
                    Connections++;
                    clientSetupAgent.Start();
                }
                catch (IOException e)
                {
                    Console.WriteLine("Accept failed");
                }
            }
        }

        //finalize

        public static void Main(string[] args)
        {
            try
            {
                MultiServer sts = new MultiServer();
                sts.Start();
            }
            catch (IOException)
            {
                Console.WriteLine("Cannot listen on server port");

            }
        }
    }
}
