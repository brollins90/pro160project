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
        public static int BotHealthBarHeight(this Bot b) { return 6; }
        public static int BotHealthBarWidthFull(this Bot b) { return 60; }
        public static int BotHealthBarWidth(this Bot b) { return (b.Health / b.MaxHealth) * b.BotHealthBarWidthFull(); }
        public static int PlayerHealthBarHeight(this Bot b) { return 26; }
        public static int PlayerHealthBarWidthFull(this Bot b) { return 200; }
        public static int PlayerHealthBarWidth(this Bot b) { return (b.Health / b.MaxHealth) * b.PlayerHealthBarWidthFull(); }
        public static int PlayerXPBarHeight(this Character c) { return 11; }
        public static int PlayerXPBarWidthFull(this Character c) { return 580; }
        public static int PlayerXPBarWidth(this Character c) { return (c.Experience / c.ExperienceNextLevel) * c.PlayerXPBarWidthFull(); }
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
        public int HeadSlots { get; set; }
        public int ChestSlots { get; set; }
        public int PantsSlots { get; set; }
        public int RingsSlots { get; set; }
        public int NeckSlots { get; set; }



        public MainWindow(bool isServer, NetworkClient netClient, int classChosen)
        {
            Console.WriteLine("{0} MainWindow - Create", System.Threading.Thread.CurrentThread.ManagedThreadId);

            Cursor myCursor = new Cursor(System.IO.Path.GetFullPath("cursor.cur"));
            this.Cursor = myCursor;

            HeadSlots = 1;
            ChestSlots = 1;
            PantsSlots = 1;
            RingsSlots = 2;
            NeckSlots = 1;



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
            //CurrentHealth.Width = (CurrentCharacter.Health / CurrentCharacter.MaxHealth) * 100;
            //CurrentExperienceBar.Width = (double)(CurrentCharacter.Experience / CurrentCharacter.ExperienceNextLevel) * ExperienceBar.Width;

            if (CurrentCharacter.ClassType == GameConstants.TYPE_CHARACTER_MAGE)
            {
                StrLabel.Content = "Int: ";
            }
            else if (CurrentCharacter.ClassType == GameConstants.TYPE_CHARACTER_ARCHER)
            {
                StrLabel.Content = "Dex: ";
            }
        }


        //public CollectionViewSource Alive { get; set; }
        private void AliveFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (e.Item as GameObject).Alive;
        }
        private void BotsFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = (e.Item as GameObject).Alive && 
                ((e.Item as GameObject).ClassType > GameConstants.TYPE_BOT_LOW &&
                (e.Item as GameObject).ClassType < GameConstants.TYPE_BOT_HIGH) ||
                ((e.Item as GameObject).ClassType > GameConstants.TYPE_CHARACTER_LOW &&
                (e.Item as GameObject).ClassType < GameConstants.TYPE_CHARACTER_HIGH &&
                (e.Item as GameObject).ID != CurrentCharacter.ID);
        }


        private void Gui_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (ShopMenu.Visibility == Visibility.Visible)
                    {
                        CloseShop();
                    }
                    else
                    {
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

        #region ShopItems

        #region Rings

        private void BuyOnyxRing_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 2;
            int GoldAmount = 300;

            if (RingsSlots == 2 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot1.Source = new BitmapImage(new Uri("Images/OnyxRing.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else if (RingsSlots == 1 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot2.Source = new BitmapImage(new Uri("Images/OnyxRing.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyZerkRing_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 2;
            int GoldAmount = 300;

            if (RingsSlots == 2 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot1.Source = new BitmapImage(new Uri("Images/ZerkRing.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else if (RingsSlots == 1 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot2.Source = new BitmapImage(new Uri("Images/ZerkRing.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyRingOfLife_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 2;
            int GoldAmount = 300;

            if (RingsSlots == 2 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot1.Source = new BitmapImage(new Uri("Images/RingOfLife.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_CONSTITUTION, StatIncrease, GoldAmount);
                //Manager.UpgradeLife(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else if (RingsSlots == 1 && CurrentCharacter.Gold >= GoldAmount)
            {
                RingsSlots -= 1;
                RingSlotsLeft.Content = "" + RingsSlots;
                RingSlot2.Source = new BitmapImage(new Uri("Images/RingOfLife.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_CONSTITUTION, StatIncrease, GoldAmount);
                //Manager.UpgradeLife(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        #endregion

        #region Amulets
        private void BuyAmmyOfLife_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 3;
            int GoldAmount = 500;

            if (NeckSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                NeckSlots -= 1;
                NeckSlotsLeft.Content = "" + NeckSlots;
                NecklaceSlot.Source = new BitmapImage(new Uri("Images/AmmyOfGlory.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_CONSTITUTION, StatIncrease, GoldAmount);
                //Manager.UpgradeLife(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyAmmyOfPower_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 3;
            int GoldAmount = 500;

            if (NeckSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                NeckSlots -= 1;
                NeckSlotsLeft.Content = "" + NeckSlots;
                NecklaceSlot.Source = new BitmapImage(new Uri("Images/AmmyOfPower.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyAmmyOfDef_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 3;
            int GoldAmount = 500;

            if (NeckSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                NeckSlots -= 1;
                NeckSlotsLeft.Content = "" + NeckSlots;
                NecklaceSlot.Source = new BitmapImage(new Uri("Images/AmmyOfDef.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        #endregion

        #region Bottom
        private void BuyDragonChaps_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 2;
            int GoldAmount = 1000;

            if (PantsSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                PantsSlots -= 1;
                PantSlotsLeft.Content = "" + PantsSlots;
                BotSlot.Source = new BitmapImage(new Uri("Images/RangeChaps.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyWizBot_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 4;
            int GoldAmount = 1000;

            if (PantsSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                PantsSlots -= 1;
                PantSlotsLeft.Content = "" + PantsSlots;
                BotSlot.Source = new BitmapImage(new Uri("Images/WizBot.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyPlatelegs_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 4;
            int GoldAmount = 1000;

            if (PantsSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                PantsSlots -= 1;
                PantSlotsLeft.Content = "" + PantsSlots;
                BotSlot.Source = new BitmapImage(new Uri("Images/PlateLegs.png", UriKind.RelativeOrAbsolute));
                BotSlot.Height = 100;
                BotSlot.Width = 100;
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }

        }

        #endregion

        #region Chest
        private void BuyDragonHide_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 4;
            int GoldAmount = 1500;

            if (ChestSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                ChestSlots -= 1;
                ChestSlotsLeft.Content = "" + ChestSlots;
                MidSlot.Source = new BitmapImage(new Uri("Images/RangeBody.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyWizTop_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 4;
            int GoldAmount = 1500;

            if (ChestSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                ChestSlots -= 1;
                ChestSlotsLeft.Content = "" + ChestSlots;
                MidSlot.Source = new BitmapImage(new Uri("Images/Wiztop.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyBreastplate_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 6;
            int GoldAmount = 1500;

            if (ChestSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                ChestSlots -= 1;
                ChestSlotsLeft.Content = "" + ChestSlots;
                MidSlot.Source = new BitmapImage(new Uri("Images/Chestplate.png", UriKind.RelativeOrAbsolute)); ;
                MidSlot.Height = 100;
                MidSlot.Width = 100;
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }



        #endregion

        #region Head

        private void BuyCoif_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 3;
            int GoldAmount = 1250;

            if (HeadSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                HeadSlots -= 1;
                HeadSlotsLeft.Content = "" + HeadSlots;
                TopSlot.Source = new BitmapImage(new Uri("Images/Coif.png", UriKind.RelativeOrAbsolute));

                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyWizHat_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 6;
            int GoldAmount = 1250;

            if (HeadSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                HeadSlots -= 1;
                HeadSlotsLeft.Content = "" + HeadSlots;
                TopSlot.Source = new BitmapImage(new Uri("Images/WizHat.png", UriKind.RelativeOrAbsolute));
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_STRENGTH, StatIncrease, GoldAmount);
                //Manager.UpgradeStr(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        private void BuyFullHelm_Click(object sender, RoutedEventArgs e)
        {
            int StatIncrease = 5;
            int GoldAmount = 1250;

            if (HeadSlots > 0 && CurrentCharacter.Gold >= GoldAmount)
            {
                HeadSlots -= 1;
                HeadSlotsLeft.Content = "" + HeadSlots;
                TopSlot.Source = new BitmapImage(new Uri("Images/FullHelm.png", UriKind.RelativeOrAbsolute)); ;
                TopSlot.Height = 100;
                TopSlot.Width = 100;
                Manager.UpgradeStat(CurrentCharacter.ID, GameConstants.STAT_DEFENSE, StatIncrease, GoldAmount);
                //Manager.UpgradeDef(CurrentCharacter, StatIncrease, GoldAmount);
            }
            else
            {

            }
        }

        #endregion
        #endregion

    }
}
