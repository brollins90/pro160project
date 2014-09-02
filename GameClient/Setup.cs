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
using System.Runtime.InteropServices;

namespace GameClient
{
    public class Setup
    {
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        [STAThread()]
        public static void Main(string[] args)
        {


            bool text = false;
            bool online = false;
            string serverName = "localhost";
            int serverPort = SimpleServer.ServerPort;
            int classChosen = GameConstants.TYPE_CHARACTER_ARCHER;

            if (text)
            {
                Main_Text(ref online, ref serverName, ref serverPort, ref classChosen);
            }
            else
            {
                Main_GUI(ref online, ref serverName, ref serverPort, ref classChosen);
            }
        }

        private static void Main_GUI(ref bool online, ref string serverName, ref int serverPort, ref int classChosen)
        {
            App app = new App();
            app.MainWindow = new MainMenu(ref online, ref serverName, ref serverPort, ref classChosen);
            app.MainWindow.Show();
            app.Run();

            Main_Connect(!online, serverName, serverPort, null, classChosen);
        }

        private static void Main_Text(ref bool online, ref string serverName, ref int serverPort, ref int classChosen)
        {
            AllocConsole();

            Console.WriteLine("Do you want to play online?");
            string onlineString = Console.ReadLine();
            onlineString = (string.IsNullOrEmpty(onlineString)) ? "no" : onlineString;
            online = onlineString.ToLower()[0] == 'y';

            if (online)
            {
                Console.WriteLine("What server do you want to connect to? [localhost]");
                serverName = Console.ReadLine();
                serverName = (string.IsNullOrEmpty(serverName)) ? "localhost" : serverName;

                Console.WriteLine("What port? [" + SimpleServer.ServerPort + "]");
                string portString = Console.ReadLine();
                serverPort = SimpleServer.ServerPort;
            }
            Console.WriteLine("What class do you want to be? [Archer]");
            string classString = Console.ReadLine();


            Main_Connect(!online, serverName, serverPort, null, classChosen);
        }

        public static void Main_Connect(bool startServer, string serverName, int serverPort, MainMenu menu, int classChosen)
        {

            if (startServer)
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

            TcpClient client = null;
            NetworkClient netClient = null;
            string line = null;
            bool done = false;
            bool isServer = false;

            try
            {
                client = new TcpClient(serverName, serverPort);
                netClient = new NetworkClient(client.GetStream());
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


            StartGameWindow(netClient, isServer, menu, classChosen);

            //netClient.GrabGui(app.MainWindow);
            //app.MainWindow.Timer.Start();

            if (isServer)
            {
                // server update thread
            }
        }

        private static void StartGameWindow(NetworkClient netClient, bool isServer, MainMenu menu, int classChosen)
        {

            if (menu == null)
            {
                // Create the GUI
                App app = new App();
            }


            MainWindow gamewindow = new MainWindow(isServer, netClient, classChosen);
            gamewindow.Show();
            menu.Hide();



            //app.MainWindow = new MainWindow(isServer, netClient, GameConstants.TYPE_CHARACTER_ARCHER);
            //app.MainWindow.Show();
            //app.Run();
        }
    }
}
