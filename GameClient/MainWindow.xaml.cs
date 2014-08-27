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

namespace GameClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public GameManager Manager {get;set;}
        public Controller CurrentController {get; set;}

        public CharacterClasses ClassChosen { get; set; }


        public MainWindow(CharacterClasses classChosen)
        {
            ClassChosen = classChosen;
            // Init the components
            InitializeComponent();
            // Every game needs a manager (instance of the game)
            Manager = new GameManager();

            this.DataContext = this;
            MainGrid.Focusable = true;
            MainGrid.Focus();

            

            // Create the interface component for the Play to submit commands
            CurrentController = new Controller();
            CurrentController.Connect(Manager);
            CurrentController.CreateCharacter(ClassChosen);

            PopulateGame();

            // create some objects to bind the HUD portion of the UI to
            CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void PopulateGame()
        {

            // Add some Bots
            //Manager.LoadWorld("");
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            GameCommands keyPressed = GameCommands.None;
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    keyPressed = GameCommands.Up;
                    CurrentController.InputListener.KeyForward = true;
                    break;

                case Key.Down:
                case Key.S:
                    keyPressed = GameCommands.Down;
                    CurrentController.InputListener.KeyBackward = true;
                    break;

                case Key.Left:
                case Key.A:
                    keyPressed = GameCommands.Left;
                    CurrentController.InputListener.KeyLeft = true;
                    break;

                case Key.Right:
                case Key.D:
                    keyPressed = GameCommands.Right;
                    CurrentController.InputListener.KeyRight = true;
                    break;
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Space:
                case Key.T:
                    keyPressed = GameCommands.Space;
                    CurrentController.InputListener.KeyFire = true;
                    break;
            }
            Console.WriteLine("{0} {1} KeyDown: {2}", (int)AppDomain.GetCurrentThreadId(), Environment.TickCount, keyPressed);
//            Console.WriteLine("KeyDown: {0}", keyPressed);
            Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, keyPressed, Environment.TickCount));
            //CurrentController.KeyDown(keyPressed);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {            
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void LevelUpButton(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentCharacter as Character).LevelUp();
            CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
            
        }

        private void TakeDamage(object sender, RoutedEventArgs e)
        {

            Random rand = new Random();

            (CurrentController.CurrentCharacter as Character).Health = (CurrentController.CurrentCharacter as Character).Health - rand.Next(10) + 1;

            double healthleft = (double)((double)(CurrentController.CurrentCharacter as Character).Health / (double)(CurrentController.CurrentCharacter as Character).MaxHealth) * 100;

            if (healthleft <= 0)
            {
                CurrentHealth.Width = 0;

                MessageBox.Show("Game Over. You were level " + (CurrentController.CurrentCharacter as Character).Level + ", when you died");
                MainMenu mainmenu = new MainMenu();
                mainmenu.Show();
                this.Hide();
                
            }
            else
            {
                CurrentHealth.Width = healthleft;
            }


        }

        private void GainExp(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentCharacter as Character).Experience += 10;

            if ((CurrentController.CurrentCharacter as Character).Experience == (CurrentController.CurrentCharacter as Character).ExperienceCap)
            {
                (CurrentController.CurrentCharacter as Character).LevelUp();
                CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void GainMoreExp(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentCharacter as Character).Experience += 20;

            if ((CurrentController.CurrentCharacter as Character).Experience >= (CurrentController.CurrentCharacter as Character).ExperienceCap)
            {
                int expleft = (CurrentController.CurrentCharacter as Character).Experience - (CurrentController.CurrentCharacter as Character).ExperienceCap;
                (CurrentController.CurrentCharacter as Character).LevelUp();
                (CurrentController.CurrentCharacter as Character).Experience = expleft;
                CurrentHealth.Width = ((CurrentController.CurrentCharacter as Character).Health / (CurrentController.CurrentCharacter as Character).MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentCharacter as Character).Experience / (double)(CurrentController.CurrentCharacter as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void GainGold(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentCharacter as Character).Gold += 10;
        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("down");
            if (e.ChangedButton == MouseButton.Left)
            {
                Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.LeftClick, Environment.TickCount, e.GetPosition(this)));
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.RightClick, Environment.TickCount, e.GetPosition(this)));
            }
        }

        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("move");
            Manager.SubmitMove(new GameCommand(CurrentController.GameObjectID, GameCommands.MouseMove, Environment.TickCount, e.GetPosition(this)));
        }
        
        //Close shop
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShopMenu.Visibility = Visibility.Collapsed;
            NotEnoughGold.Visibility = Visibility.Collapsed;
        }

        //Open shop
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShopMenu.Visibility = Visibility.Visible;
            NotEnoughGold.Visibility = Visibility.Collapsed;
        }

        //Reinforce armor plating
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NotEnoughGold.Visibility = Visibility.Collapsed;
            if (CurrentController.CurrentCharacter.Gold >= 20)
            {
                CurrentController.CurrentCharacter.Gold -= 20;
                CurrentController.CurrentCharacter.Defense += 1;
            }
            else
            {
                NotEnoughGold.Visibility = Visibility.Visible;
            }
        }

        //Reforge weapon
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NotEnoughGold.Visibility = Visibility.Collapsed;
            if (CurrentController.CurrentCharacter.Gold >= 30)
            {
                CurrentController.CurrentCharacter.Gold -= 30;
                CurrentController.CurrentCharacter.Strength += 1;
            }
            else
            {
                NotEnoughGold.Visibility = Visibility.Visible;
            }
        }

        //Drink magic potion
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NotEnoughGold.Visibility = Visibility.Collapsed;
            if (CurrentController.CurrentCharacter.Gold >= 50)
            {
                CurrentController.CurrentCharacter.Gold -= 50;
                CurrentController.CurrentCharacter.Constitution += 1;
            }
            else
            {
                NotEnoughGold.Visibility = Visibility.Visible;
            }
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    //keyPressed = GameCommands.Up;
                    CurrentController.InputListener.KeyForward = false;
                    break;

                case Key.Down:
                case Key.S:
                    //keyPressed = GameCommands.Down;
                    CurrentController.InputListener.KeyBackward = false;
                    break;

                case Key.Left:
                case Key.A:
                    //keyPressed = GameCommands.Left;
                    CurrentController.InputListener.KeyLeft = false;
                    break;

                case Key.Right:
                case Key.D:
                    //keyPressed = GameCommands.Right;
                    CurrentController.InputListener.KeyRight = false;
                    break;
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Space:
                case Key.T:
                    //keyPressed = GameCommands.Space;
                    CurrentController.InputListener.KeyFire = false;
                    break;
            }
        }
    }
}
