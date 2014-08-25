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

        public MainWindow()
        {
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
            CurrentController.CreateCharacter();

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
                    break;

                case Key.Down:
                case Key.S:
                    keyPressed = GameCommands.Down;
                    break;

                case Key.Left:
                case Key.A:
                    keyPressed = GameCommands.Left;
                    break;

                case Key.Right:
                case Key.D:
                    keyPressed = GameCommands.Right;
                    break;
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Space:
                case Key.T:
                    keyPressed = GameCommands.Space;
                    break;
            }
            Console.WriteLine("KeyDown: {0}", keyPressed);
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

        public void HealthBarCalculation()
        {
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
    }
}
