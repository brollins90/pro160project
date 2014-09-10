using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using GameCode;
using GameCode.Models;
using GameServer;
using System.Windows;

namespace GameClient
{
    /// <summary>
    /// Setup code
    /// </summary>
    public class Setup
    {
        /// <summary>
        /// We need to import the console methods manually since this is a WPF application
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int AllocConsole();

        /// <summary>
        /// Start the setup program
        /// </summary>
        /// <param name="args"></param>
        [STAThread()]
        public static void Main(string[] args)
        {
            bool text = false;
            bool online = false;
            string serverName = "localhost";
            int serverPort = SimpleServer.ServerPort;
            int classChosen = GameConstants.TYPE_CHARACTER_ARCHER;

            // Do we want a text based setup?
            if (text)
            {
                Main_Text(ref online, ref serverName, ref serverPort, ref classChosen);
            }
            else // GUI setup
            {
                Main_GUI(ref online, ref serverName, ref serverPort, ref classChosen);
            }
        }

        /// <summary>
        /// Start the GUI setup menu
        /// </summary>
        /// <param name="online"></param>
        /// <param name="serverName"></param>
        /// <param name="serverPort"></param>
        /// <param name="classChosen"></param>
        private static void Main_GUI(ref bool online, ref string serverName, ref int serverPort, ref int classChosen)
        {
            SplashScreen ss = new SplashScreen(@"Images\SplashScreen.jpg");
            ss.Show(true);
            App app = new App();
            app.MainWindow = new MainMenu(ref online, ref serverName, ref serverPort, ref classChosen);
            app.MainWindow.Show();
            app.Run();

            // GO!
            Main_Connect(!online, serverName, serverPort, null, classChosen);
        }

        /// <summary>
        /// Start the Text setup menu
        /// </summary>
        /// <param name="online"></param>
        /// <param name="serverName"></param>
        /// <param name="serverPort"></param>
        /// <param name="classChosen"></param>
        private static void Main_Text(ref bool online, ref string serverName, ref int serverPort, ref int classChosen)
        {
            // Open a console
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

            // GO!
            Main_Connect(!online, serverName, serverPort, null, classChosen);
        }

        public static void Main_Connect(bool startServer, string serverName, int serverPort, MainMenu menu, int classChosen)
        {
            // If it was chosen to start a new server, spawn a server thread
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
                    // There is already a server running
                    Console.WriteLine("Could not listen on server port: {0}", ex.ToString());
                }
            }

            // Start client processes
            TcpClient client = null;
            NetworkClient netClient = null;
            string line = null;
            bool done = false;
            bool isServer = false;

            try
            {
                // Setup the network threads
                client = new TcpClient(serverName, serverPort);
                netClient = new NetworkClient(client.GetStream());
            }
            catch (IOException ex)
            {
                Console.WriteLine("Failed to connect to server, IOException: {0}", ex.ToString());
            }

            // If we didnt connect, just quit
            if (client == null)
            {
                Environment.Exit(-1);
            }

            do
            {
                try
                {
                    // read a line
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
                // if the line starts with !, then we are done
                else if (line[0] == '!')
                {
                    done = true;
                }
                // if the line starts with ?, send a response
                else if (line[0] == '?')
                {
                    Console.WriteLine(line.Substring(1));
                    string read = Console.ReadLine();
                    netClient.WriteLine(read);
                }
                // else print the line
                else
                {
                    Console.WriteLine(line);
                }
            } while (!done);

            // End with the response on whether or not you are hosting the game
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

            StartGameWindow(netClient, isServer, menu, classChosen);
        }

        /// <summary>
        /// Start the GUI windows for the game
        /// </summary>
        /// <param name="netClient"></param>
        /// <param name="isServer"></param>
        /// <param name="menu"></param>
        /// <param name="classChosen"></param>
        private static void StartGameWindow(NetworkClient netClient, bool isServer, MainMenu menu, int classChosen)
        {
            // if we used a text based setup, we need to instantiate the WPF Application
            if (menu == null)
            {
                App app = new App();
            }

            MainWindow gamewindow = new MainWindow(isServer, netClient, classChosen);
            gamewindow.Show();

            if (menu != null)
            {
                menu.Hide();
            }
        }
    }
}
