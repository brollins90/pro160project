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
        private GameManager _Manager;
        public GameManager Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }

        private Controller _CurrentController;

        public Controller CurrentController
        {
            get { return _CurrentController; }
            set { _CurrentController = value; }
        }
        

        public MainWindow()
        {
            InitializeComponent();
            Manager = new GameManager();
            this.DataContext = this;

            MainGrid.Focusable = true;
            MainGrid.Focus();

            CurrentController = new Controller();
            CurrentController.Connect(Manager);
            CurrentController.CreateCharacter();

            Manager.AddNPC();
            //Manager.World.Objects.Add(new Character() { Position = new System.Drawing.Point(200,200) });


            CurrentHealth.Width = (CurrentController.CurrentCharacter.CurrentHealth / CurrentController.CurrentCharacter.MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)CurrentController.CurrentCharacter.Experience / (double)CurrentController.CurrentCharacter.ExperienceCap) * ExperienceBar.Width;
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

                case Key.P:
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
            CurrentController.CurrentCharacter.LevelUp();
            CurrentHealth.Width = (CurrentController.CurrentCharacter.CurrentHealth / CurrentController.CurrentCharacter.MaxHealth) * 100;
            CurrentExperienceBar.Width = (double)((double)CurrentController.CurrentCharacter.Experience / (double)CurrentController.CurrentCharacter.ExperienceCap) * ExperienceBar.Width;
            
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
            CurrentController.CurrentCharacter.Experience += 10;

            if (CurrentController.CurrentCharacter.Experience == CurrentController.CurrentCharacter.ExperienceCap)
            {
                CurrentController.CurrentCharacter.LevelUp();
                CurrentHealth.Width = (CurrentController.CurrentCharacter.CurrentHealth / CurrentController.CurrentCharacter.MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)CurrentController.CurrentCharacter.Experience / (double)CurrentController.CurrentCharacter.ExperienceCap) * ExperienceBar.Width;
        }

        private void GainMoreExp(object sender, RoutedEventArgs e)
        {
            CurrentController.CurrentCharacter.Experience += 20;

            if (CurrentController.CurrentCharacter.Experience >= CurrentController.CurrentCharacter.ExperienceCap)
            {
                int expleft = CurrentController.CurrentCharacter.Experience - CurrentController.CurrentCharacter.ExperienceCap;
                CurrentController.CurrentCharacter.LevelUp();
                CurrentController.CurrentCharacter.Experience = expleft;
                CurrentHealth.Width = (CurrentController.CurrentCharacter.CurrentHealth / CurrentController.CurrentCharacter.MaxHealth) * 100;
            }
            CurrentExperienceBar.Width = (double)((double)CurrentController.CurrentCharacter.Experience / (double)CurrentController.CurrentCharacter.ExperienceCap) * ExperienceBar.Width;
        }

        private void GainGold(object sender, RoutedEventArgs e)
        {
            CurrentController.CurrentCharacter.Gold += 10;
        }
    }
}
