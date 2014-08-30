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
            string serverName ="";
            int serverPort = SimpleServer.ServerPort;

            Console.WriteLine("Do you want to play online?");
            string onlineString = "no";// Console.ReadLine();
            bool online = onlineString.ToLower()[0] == 'y';

            if (online)
            {
                Console.WriteLine("What server do you want to connect to? [localhost]");
                serverName = Console.ReadLine();
                serverName = (string.IsNullOrEmpty(serverName)) ? "localhost" : serverName;

                Console.WriteLine("What port? [4444]");
                string portString = Console.ReadLine();
                serverPort = SimpleServer.ServerPort;
            }
            //bool playOnline = false;

            if (!online)
            {
                try
                {
                    SimpleServer simple = new SimpleServer();
                    new Thread(simple.Start)
                    {
                        IsBackground = true
                    }.Start();
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Could not listen on server port: {0}", ex.ToString());
                }
            }
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

            // I already have my network client

            // Create the GUI
            App app = new App();
            app.MainWindow = new MainWindow(isServer, netClient, GameConstants.TYPE_CHARACTER_ARCHER);
            app.MainWindow.Show();
            app.Run();

            //netClient.GrabGui(app.MainWindow);
            //app.MainWindow.Timer.Start();

            if (isServer)
            {
                // server update thread
            }
        }
    }
}
