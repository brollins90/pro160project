using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using GameCode.Models;
using GameCode;

namespace GameClient
{
    public class Setup
    {
        [STAThread()]
        public static void Main(string[] args)
        {
            string serverName = "localhost";
            int serverPort = SimpleServer.ServerPort;
            //bool playOnline = false;

            //if (!playOnline)
            //{
            //    try
            //    {
            //        SimpleServer simple = new SimpleServer();
            //        new Thread(simple.Start).Start();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Could not listen one server port: {0}", ex.ToString());
            //    }
            //}
            Main_Text(serverName, serverPort);
        }

        public static void Main_Text(string servername, int serverport)
        {
            TcpClient client = null;
            NetworkClient netClient = null;
            string line = null;
            bool done = false;
            bool isServer = false;

            try
            {
                client = new TcpClient(servername, serverport);
                netClient = new NetworkClient(client);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Failed to connect to server, IOException: {0}", ex.ToString());
            }

            if (client == null)
            {
                Environment.Exit(-1);
            }

            do
            {
                try
                {
                    line = netClient.ReadLine();
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Lost connection to server, IOException: {0}", ex.ToString());
                }

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("");
                }
                else if (line[0] == '!')
                {
                    done = true;
                }
                else if (line[0] == '?')
                {
                    Console.WriteLine(line.Substring(1));
                    string read = Console.ReadLine();
                    netClient.WriteLine(read);
                }
                else
                {
                    Console.WriteLine(line);
                }
            } while (!done);

            if ("!is server".Equals(line))
            {
                isServer = true;
            }
            else if ("!not server".Equals(line))
            {
                isServer = false;
            }
            else
            {
                Console.WriteLine("Server returned an invalid initial response...");
                Environment.Exit(-1);
            }

            App app = new App();
            app.MainWindow = new MainWindow(GameConstants.TYPE_CHARACTER_ARCHER, netClient, isServer);
            app.MainWindow.Show();
            app.Run();
        }
    }
}
