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

            PopulateGame();

            // create some objects to bind the HUD portion of the UI to
            CurrentHealth.Width = ((CurrentController.CurrentObject as Character).CurrentHealth / (CurrentController.CurrentObject as Character).MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentObject as Character).Experience / (double)(CurrentController.CurrentObject as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void PopulateGame()
        {
            CurrentController.CreateCharacter();

            // Add some Bots
            Manager.AddNPC();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            GameCommands keyPressed = GameCommands.None;
            switch (e.Key)
            {
                case Key.Up:
                    keyPressed = GameCommands.Up;
                    break;

                case Key.Down:
                    keyPressed = GameCommands.Down;
                    break;

                case Key.Left:
                    keyPressed = GameCommands.Left;
                    break;

                case Key.Right:
                    keyPressed = GameCommands.Right;
                    break;

                case Key.Space:
                case Key.T:
                    keyPressed = GameCommands.Space;
                    break;
            }
            Console.WriteLine("KeyDown: {0}", keyPressed);
            CurrentController.KeyDown(keyPressed);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {            
            base.OnClosing(e);
            Application.Current.Shutdown();
        }

        private void LevelUpButton(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentObject as Character).LevelUp();
            CurrentHealth.Width = ((CurrentController.CurrentObject as Character).CurrentHealth / (CurrentController.CurrentObject as Character).MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentObject as Character).Experience / (double)(CurrentController.CurrentObject as Character).ExperienceCap) * ExperienceBar.Width;
            
        }

        private void TakeDamage(object sender, RoutedEventArgs e)
        {

            Random rand = new Random();

            CurrentController.CurrentCharacter.CurrentHealth = CurrentController.CurrentCharacter.CurrentHealth - rand.Next(10) + 1;

            double healthleft = (double) ((double) CurrentController.CurrentCharacter.CurrentHealth / (double) CurrentController.CurrentCharacter.MaxHealth) * 100;

            if (healthleft <= 0)
            {
                CurrentHealth.Width = 0;

                MessageBox.Show("Game Over. You were level " + CurrentController.CurrentCharacter.Level + ", when you died");
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
            (CurrentController.CurrentObject as Character).Experience += 10;

            if ((CurrentController.CurrentObject as Character).Experience == (CurrentController.CurrentObject as Character).ExperienceCap)
            {
                (CurrentController.CurrentObject as Character).LevelUp();
                CurrentHealth.Width = ((CurrentController.CurrentObject as Character).CurrentHealth / (CurrentController.CurrentObject as Character).MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentObject as Character).Experience / (double)(CurrentController.CurrentObject as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void GainMoreExp(object sender, RoutedEventArgs e)
        {
            (CurrentController.CurrentObject as Character).Experience += 20;

            if ((CurrentController.CurrentObject as Character).Experience >= (CurrentController.CurrentObject as Character).ExperienceCap)
            {
                int expleft = (CurrentController.CurrentObject as Character).Experience - (CurrentController.CurrentObject as Character).ExperienceCap;
                (CurrentController.CurrentObject as Character).LevelUp();
                (CurrentController.CurrentObject as Character).Experience = expleft;
                CurrentHealth.Width = ((CurrentController.CurrentObject as Character).CurrentHealth / (CurrentController.CurrentObject as Character).MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)(CurrentController.CurrentObject as Character).Experience / (double)(CurrentController.CurrentObject as Character).ExperienceCap) * ExperienceBar.Width;
        }

        private void GainGold(object sender, RoutedEventArgs e)
        {
            CurrentController.CurrentCharacter.Gold += 10;
        }
    }
}
