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
            GameCommands keyPressed = GameCommands.Down;
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
            CurrentController.CurrentCharacter.DamageTaken = rand.Next(10) + 1;
            CurrentController.CurrentCharacter.CurrentHealth = CurrentController.CurrentCharacter.CurrentHealth;
            CurrentHealth.Width = (double) ((double) CurrentController.CurrentCharacter.CurrentHealth / (double) CurrentController.CurrentCharacter.MaxHealth) * 100;

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
    }
}
