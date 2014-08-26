using System;
using System.Collections.Generic;
using System.Linq;
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
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void Mage_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow();
            gamewindow.Show();
            this.Hide();
        }

        private void Archer_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow();
            gamewindow.Show();
            this.Hide();
        }

        private void Fighter_Click(object sender, RoutedEventArgs e)
        {

            MainWindow gamewindow = new MainWindow();
            gamewindow.Show();
            this.Hide();
        }
    }
}
