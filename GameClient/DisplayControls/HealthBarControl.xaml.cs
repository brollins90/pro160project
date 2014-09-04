//using GameCode.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

//namespace GameClient.DisplayControls
//{
//    /// <summary>
//    /// Interaction logic for HealthBarControl.xaml
//    /// </summary>
//    public partial class HealthBarControl : UserControl, INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        //public enum GameObjectType { Bot, Player, Projectile };

//        public void FirePropertyChanged(String propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                //Console.WriteLine("ColorSelectorModel.FirePropertyChanged({0})", propertyName);
//                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//        public Bot Bot
//        {
//            get { return (Bot)GetValue(BotProperty); }
//            set
//            {
//                SetValue(BotProperty, value);
//                value.PropertyChanged += new PropertyChangedEventHandler(Bot_PropertyChanged);
//                FirePropertyChanged("Bot");
//            }
//        }

//        //private void OnPropertyChanged(string p)
//        //{
//        //    PropertyChangedEventHandler handler = PropertyChanged;
//        //    if (handler != null) {
//        //        handler(this, new PropertyChangedEventArgs(p));
//        //    }
//        //}

//        public static readonly DependencyProperty BotProperty =
//            DependencyProperty.Register(
//            "Bot",
//            typeof(Bot),
//            typeof(HealthBarControl),
//            new FrameworkPropertyMetadata()
//            {
//            }
//            );

//        private static void OnBotPropertyChangedCallBack(
//            DependencyObject sender, DependencyPropertyChangedEventArgs e)
//        {
//            HealthBarControl c = sender as HealthBarControl;
//            if (c != null) {
//                c.OnBotPropertyChanged();
//            }
//        }

//        private void OnBotPropertyChanged()
//        {
//            OnPropertyChanged(new DependencyPropertyChangedEventArgs(BotProperty, 0, 0));
//        }

//        //public event PropertyChangedEventHandler PropertyChanged;
//        //void SetValueDp(DependencyProperty property, object value,
//        //    [System.Runtime.CompilerServices.CallerMemberName] string p = null)
//        //{
//        //    SetValue(property, value);
//        //    if (PropertyChanged != null)
//        //    {
//        //        PropertyChanged(this, new PropertyChangedEventArgs(p));
//        //    }
//        //}


//        public int HealthBarHeight
//        {
//            get { return 6; }
//        }
//        public int HealthBarWidthFull
//        {
//            get { return 60; }
//        }
//        public int HealthBarWidth
//        {
//            get { return (Bot.Health / Bot.MaxHealth) * HealthBarWidthFull; }
//        }

//        private void Bot_PropertyChanged(Object sender,
//              PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName == "Health")
//            {
//                FirePropertyChanged("HealthBarWidth");
//            }
//        }
        
    

//        public HealthBarControl()
//        {
//            InitializeComponent();
//            (this.Content as FrameworkElement).DataContext = this;
            
//        }
//    }
//}
