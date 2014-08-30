using GameCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace GameClient
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    /// 
    public partial class MainMenu : Window
    {
        private NetworkClient NetClient;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void ExitGameButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PlayGameButton(object sender, RoutedEventArgs e)
        {
            PlayGame.Visibility = Visibility.Hidden;
            ExitGame.Visibility = Visibility.Hidden;
            PlayVsAI.Visibility = Visibility.Visible;
            PlayOnline.Visibility = Visibility.Visible;
        }

        private void PlayVsAIButton(object sender, RoutedEventArgs e)
        {
            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            Archer.Visibility = Visibility.Visible;
            Mage.Visibility = Visibility.Visible;
            Fighter.Visibility = Visibility.Visible;

        }

        private void PlayOnlineButton(object sender, RoutedEventArgs e)
        {
            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            Archer.Visibility = Visibility.Hidden;
            Mage.Visibility = Visibility.Hidden;
            Fighter.Visibility = Visibility.Hidden;
            ServerNameText.Visibility = Visibility.Visible;
            ConnectToServer.Visibility = Visibility.Visible;

        }

        private void ConnectToServerButton(object sender, RoutedEventArgs e)
        {
            ServerNameText.Visibility = Visibility.Hidden;
            ConnectToServer.Visibility = Visibility.Hidden;
            ServerInfoText.Visibility = Visibility.Visible;
            ServerSend.Visibility = Visibility.Visible;
            ServerSendText.Visibility = Visibility.Visible;

            try
            {
                TcpClient client = new TcpClient(ServerNameText.Text, 3333);
                NetClient = new NetworkClient(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
                NetClient = null;
            }
            if (NetClient == null)
            {
                Environment.Exit(1);
            }
            ListenForServer();
        }

        private void ListenForServer()
        {
            bool done = false;
            string line = "";
            do
            {
                try
                {
                    line = NetClient.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lost connection to server: {0}", ex.ToString());
                    Environment.Exit(1);
                }
                if (string.IsNullOrEmpty(line))
                {
                    ServerInfoText.AppendText("\n");
                }
                else if (line[0] == '!')
                {
                    done = true;
                }
                else if (line[0] == '?')
                {
                    ServerInfoText.AppendText(line.Substring(1) + "\n");
                    Console.WriteLine(line.Substring(1));
                    done = true;
                    //String inLine = Console.ReadLine();
                    //sw.WriteLine(inLine);
                    //sw.Flush();
                }
                else
                {
                    ServerInfoText.AppendText(line + "\n");
                    Console.WriteLine(line);
                }

            } while (!done);
        }

        private void ServerSendButton(object sender, RoutedEventArgs e)
        {

            String inLine = ServerSendText.Text;
            NetClient.WriteLine(inLine);
            ServerSendText.Text = "";
            ListenForServer();
        }
        

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void Mage_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow(true, NetClient, GameConstants.TYPE_CHARACTER_MAGE);
            gamewindow.Show();
            this.Hide();
        }

        private void Archer_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow(true, NetClient, GameConstants.TYPE_CHARACTER_ARCHER);            
            gamewindow.Show();
            this.Hide();
        }

        private void Fighter_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow(true, NetClient, GameConstants.TYPE_CHARACTER_FIGHTER);
            gamewindow.Show();
            this.Hide();
        }
    }
}
