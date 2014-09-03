using GameCode;
using GameServer;
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
        //private NetworkClient NetClient;
        //private Setup setup;
        private bool online;
        private string serverName;
        private int serverPort;
        private int classChosen;

        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(ref bool online, ref string serverName, ref int serverPort, ref int classType)
        {
            //this.setup = setup;
            this.online = online;
            this.serverName = serverName;
            this.serverPort = serverPort;
            this.classChosen = classType;

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
            online = false;

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
            online = true;

        }

        private void ConnectToServerButton(object sender, RoutedEventArgs e)
        {
            //ServerNameText.Visibility = Visibility.Hidden;
            //ConnectToServer.Visibility = Visibility.Hidden;
            //ServerInfoText.Visibility = Visibility.Visible;
            ////ServerSend.Visibility = Visibility.Visible;
            //ServerSendText.Visibility = Visibility.Visible;

            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            Archer.Visibility = Visibility.Visible;
            Mage.Visibility = Visibility.Visible;
            Fighter.Visibility = Visibility.Visible;

            serverName = ServerNameText.Text;
            serverPort = SimpleServer.ServerPort;
        }
        

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void Mage_Click(object sender, RoutedEventArgs e)
        {
            this.classChosen = GameConstants.TYPE_CHARACTER_MAGE;
            Setup.Main_Connect(!online, serverName, serverPort, this, classChosen);
        }

        private void Archer_Click(object sender, RoutedEventArgs e)
        {
            this.classChosen = GameConstants.TYPE_CHARACTER_ARCHER;
            Setup.Main_Connect(!online, serverName, serverPort, this, classChosen);
            //MainWindow gamewindow = new MainWindow(true, NetClient, GameConstants.TYPE_CHARACTER_ARCHER);            
            //gamewindow.Show();
            //this.Hide();
        }

        private void Fighter_Click(object sender, RoutedEventArgs e)
        {
            this.classChosen = GameConstants.TYPE_CHARACTER_FIGHTER;
            Setup.Main_Connect(!online, serverName, serverPort, this, classChosen);
            //MainWindow gamewindow = new MainWindow(true, NetClient, GameConstants.TYPE_CHARACTER_FIGHTER);
            //gamewindow.Show();
            //this.Hide();
        }
    }
}
