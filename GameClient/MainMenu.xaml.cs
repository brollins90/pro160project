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
    /// Setup window.  Select the options for the game
    /// </summary>
    /// 
    public partial class MainMenu : Window
    {
        private bool online;
        private string serverName;
        private int serverPort;
        private int classChosen;

        public MainMenu(ref bool online, ref string serverName, ref int serverPort, ref int classType)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.online = online;
            this.serverName = serverName;
            this.serverPort = serverPort;
            this.classChosen = classType;

            InitializeComponent();
        }

        /// <summary>
        /// Exit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitGameButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Play a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayGameButton(object sender, RoutedEventArgs e)
        {
            PlayGame.Visibility = Visibility.Hidden;
            ExitGame.Visibility = Visibility.Hidden;
            PlayVsAI.Visibility = Visibility.Visible;
            PlayOnline.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Play alone or host a server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayVsAIButton(object sender, RoutedEventArgs e)
        {
            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            ArcherButton.Visibility = Visibility.Visible;
            MageButton.Visibility = Visibility.Visible;
            FighterButton.Visibility = Visibility.Visible;
            online = false;

        }

        /// <summary>
        /// Play as a client on another server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayOnlineButton(object sender, RoutedEventArgs e)
        {
            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            ArcherButton.Visibility = Visibility.Hidden;
            MageButton.Visibility = Visibility.Hidden;
            FighterButton.Visibility = Visibility.Hidden;
            ServerNameText.Visibility = Visibility.Visible;
            ConnectToServer.Visibility = Visibility.Visible;
            online = true;

        }

        /// <summary>
        /// Connect to a server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectToServerButton(object sender, RoutedEventArgs e)
        {
            PlayVsAI.Visibility = Visibility.Hidden;
            PlayOnline.Visibility = Visibility.Hidden;
            ArcherButton.Visibility = Visibility.Visible;
            MageButton.Visibility = Visibility.Visible;
            FighterButton.Visibility = Visibility.Visible;

            serverName = ServerNameText.Text;
            serverPort = SimpleServer.ServerPort;
        }
        
        /// <summary>
        /// What to do on closing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            // We are not far enough in to the game, so just kill it
            Environment.Exit(0);
        }

        /// <summary>
        /// Start a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseClass_Click(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(MageButton))
            {
                this.classChosen = GameConstants.TYPE_CHARACTER_MAGE;
            }
            else if (sender.Equals(ArcherButton))
            {
                this.classChosen = GameConstants.TYPE_CHARACTER_ARCHER;
            }
            else if (sender.Equals(FighterButton))
            {
                this.classChosen = GameConstants.TYPE_CHARACTER_FIGHTER;
            }
            Setup.Main_Connect(!online, serverName, serverPort, this, classChosen);
        }
    }
}
