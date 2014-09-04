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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameCode;
using GameCode.Models;
using System.Net.Sockets;
using GameCode.Helpers;

namespace GameClient
{
    public static class DisplayExtensions
    {
        public static int HealthBarHeight(this Bot b)
        {
            return 6;
        }
        public static int HealthBarWidthFull(this Bot b)
        {
            return 60;
        }
        public static int HealthBarWidth(this Bot b)
        {
            return (b.Health / b.MaxHealth) * b.HealthBarWidthFull();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public GameManager Manager { get; set; }
        //public int ClassChosen { get; set; }
        public InputListener GL { get; set; }
        public Character CurrentCharacter { get; set; }


        public MainWindow(bool isServer, NetworkClient netClient, int classChosen)
        {
            Console.WriteLine("{0} MainWindow - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);

            Cursor myCursor = new Cursor(System.IO.Path.GetFullPath("cursor.cur"));
            this.Cursor = myCursor;

            GL = new InputListener();

            this.KeyDown += GL.Gui_KeyDown;
            this.KeyDown += this.Gui_KeyDown;
            this.KeyUp += GL.Gui_KeyUp;
            this.MouseDown += GL.Gui_MouseDown;
            this.MouseMove += GL.Gui_MouseMove;
            this.MouseUp += GL.Gui_MouseUp;

            // Init the components
            InitializeComponent();

            // Every game needs a manager (instance of the game)
            Manager = new GameManager(isServer, netClient, GL, classChosen);

            //Alive.Source = Manager.World;
            //Alive.Filter += AliveFilter;

            //// Create my character
            CurrentCharacter = Manager.GetCurrentCharacter();

            this.DataContext = this;
            MainGrid.Focusable = true;
            MainGrid.Focus();

            // create some objects to bind the HUD portion of the UI to
            CurrentHealth.Width = (CurrentCharacter.Health / CurrentCharacter.MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)(CurrentCharacter.Experience / CurrentCharacter.ExperienceCap) * ExperienceBar.Width;
        }


        //public CollectionViewSource Alive { get; set; }
        private void AliveFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (e.Item as GameObject).Alive;
        }
        private void BotsFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (e.Item as GameObject).Alive && (e.Item as GameObject).ClassType > GameConstants.TYPE_BOT_LOW && (e.Item as GameObject).ClassType < GameConstants.TYPE_BOT_HIGH;
        }


        private void Gui_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (ShopMenu.Visibility == Visibility.Visible) {
                        CloseShop();
                    } else {
                        QuitMenu.Visibility = Visibility.Visible;
                    }
                    break;
                case Key.O:
                case Key.P:
                case Key.F10:
                    OpenShop();
                    break;
            }
        }

        private void CloseShop()
        {            
            ShopMenu.Visibility = Visibility.Collapsed;
        }

        private void OpenShop()
        {
            ShopMenu.Visibility = Visibility.Visible;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
                    Application.Current.Shutdown();
            }

        //Close shop
        private void CloseShopClick(object sender, RoutedEventArgs e)
        {
            CloseShop();
        }





























        private void ConfirmQuit(object sender, RoutedEventArgs e)
        {
            Manager.EndGame();
        }

        private void NoQuit(object sender, RoutedEventArgs e)
        {
            QuitMenu.Visibility = Visibility.Collapsed;
        }

        ////ends game if player is dead
        //public void CheckIfDead()
        //{
        //    if (CurrentController.CurrentCharacter.Health <= 0)
        //    {
        //        CurrentHealth.Width = 0;
        //        GameOver.Visibility = Visibility.Visible;
        //        MessageBox.Show("Game Over. You were level " + (CurrentController.CurrentCharacter as Character).Level + ", when you died");
        //        MainMenu mainmenu = new MainMenu();
        //        mainmenu.Show();
        //        this.Hide();
        //    }
        //    else
        //    {
        //        CurrentHealth.Width = CurrentController.CurrentCharacter.Health;
        //    }
        //}

        //private void LevelUpButton(object sender, RoutedEventArgs e)
        //{
        //    (CurrentController.CurrentCharacter as Character).LevelUp();
        //    CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
        //    CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        //    CheckIfDead();
        //}

        //private void TakeDamage(object sender, RoutedEventArgs e)
        //{
        //    Random rand = new Random();

        //    (CurrentController.CurrentCharacter as Character).Health = (CurrentController.CurrentCharacter as Character).Health - rand.Next(10) + 1;

        //    double healthleft = (double)((double)(CurrentController.CurrentCharacter as Character).Health / (double)(CurrentController.CurrentCharacter as Character).MaxHealth) * 100;

        //    if (healthleft <= 0)
        //    {
        //        CurrentHealth.Width = 0;
        //        GameOver.Visibility = Visibility.Visible;
        //        MessageBox.Show("Game Over. You were level " + (CurrentController.CurrentCharacter as Character).Level + ", when you died");
        //        MainMenu mainmenu = new MainMenu();
        //        mainmenu.Show();
        //        this.Hide();
        //    }
        //    else
        //    {
        //        CurrentHealth.Width = healthleft;
        //    }
        //    CheckIfDead();
        //}

        //private void GainExp(object sender, RoutedEventArgs e)
        //{
        //    (CurrentController.CurrentCharacter as Character).Experience += 10;

        //    if ((CurrentController.CurrentCharacter as Character).Experience == (CurrentController.CurrentCharacter as Character).ExperienceCap)
        //    {
        //        (CurrentController.CurrentCharacter as Character).LevelUp();
        //        CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
        //    }
        //    CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        //    CheckIfDead();
        //}

        //private void GainMoreExp(object sender, RoutedEventArgs e)
        //{
        //    (CurrentController.CurrentCharacter as Character).Experience += 20;

        //    if ((CurrentController.CurrentCharacter as Character).Experience >= (CurrentController.CurrentCharacter as Character).ExperienceCap)
        //    {
        //        int expleft = (CurrentController.CurrentCharacter as Character).Experience - (CurrentController.CurrentCharacter as Character).ExperienceCap;
        //        (CurrentController.CurrentCharacter as Character).LevelUp();
        //        (CurrentController.CurrentCharacter as Character).Experience = expleft;
        //        CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
        //    }
        //    CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        //    CheckIfDead();
        //}

        //private void GainGold(object sender, RoutedEventArgs e)
        //{
        //    (CurrentController.CurrentCharacter as Character).Gold += 10;
        //    CheckIfDead();
        //}


//        private void Grid_KeyDown(object sender, KeyEventArgs e)
//        {
//            GameCommands keyPressed = GameCommands.None;
//            switch (e.Key)
//            {
//                case Key.Up:
//                case Key.W:
//                    keyPressed = GameCommands.Up;
//                    CurrentController.InputListener.KeyForward = true;
//                    break;

//                case Key.Down:
//                case Key.S:
//                    keyPressed = GameCommands.Down;
//                    CurrentController.InputListener.KeyBackward = true;
//                    break;

//                case Key.Left:
//                case Key.A:
//                    keyPressed = GameCommands.Left;
//                    CurrentController.InputListener.KeyLeft = true;
//                    break;

//                case Key.Right:
//                case Key.D:
//                    keyPressed = GameCommands.Right;
//                    CurrentController.InputListener.KeyRight = true;
//                    break;
//                case Key.Escape:
//                    Application.Current.Shutdown();
//                    break;
//                case Key.Space:
//                case Key.T:
//                    keyPressed = GameCommands.Space;
//                    CurrentController.InputListener.KeyAttack = true;
//                    break;
//            }
//            //Console.WriteLine("{0} {1} KeyDown: {2}", (int)AppDomain.GetCurrentThreadId(), Environment.TickCount, keyPressed);
////            Console.WriteLine("KeyDown: {0}", keyPressed);
//            Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, keyPressed, Environment.TickCount));
//            //CurrentController.KeyDown(keyPressed);
//            CheckIfDead();
//        }

        //private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Console.WriteLine("down");
        //    if (e.ChangedButton == MouseButton.Left)
        //    {
        //        Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.LeftClick, Environment.TickCount, e.GetPosition(this)));
        //    }
        //    else if (e.ChangedButton == MouseButton.Right)
        //    {
        //        Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.RightClick, Environment.TickCount, e.GetPosition(this)));
        //    }
        //    CheckIfDead();
        //}

        //private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Console.WriteLine("move");
        //    Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.MouseMove, Environment.TickCount, e.GetPosition(this)));
        //    CheckIfDead();
        //}

        //private void Window_KeyUp(object sender, KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.Up:
        //        case Key.W:
        //            //keyPressed = GameCommands.Up;
        //            CurrentController.InputListener.KeyForward = false;
        //            break;

        //        case Key.Down:
        //        case Key.S:
        //            //keyPressed = GameCommands.Down;
        //            CurrentController.InputListener.KeyBackward = false;
        //            break;

        //        case Key.Left:
        //        case Key.A:
        //            //keyPressed = GameCommands.Left;
        //            CurrentController.InputListener.KeyLeft = false;
        //            break;

        //        case Key.Right:
        //        case Key.D:
        //            //keyPressed = GameCommands.Right;
        //            CurrentController.InputListener.KeyRight = false;
        //            break;
        //        case Key.Escape:
        //            Application.Current.Shutdown();
        //            break;
        //        case Key.Space:
        //        case Key.T:
        //            //keyPressed = GameCommands.Space;
        //            CurrentController.InputListener.KeyAttack = false;
        //            break;
        //    }
        //    ShopMenu.Visibility = Visibility.Collapsed;
        //    NotEnoughGold.Visibility = Visibility.Collapsed;
        //    CheckIfDead();
        //}

    }
}
