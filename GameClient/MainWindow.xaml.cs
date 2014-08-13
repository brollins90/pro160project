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

        public MainWindow()
        {
            InitializeComponent();
            Manager = new GameManager();
            this.DataContext = Manager;

            Manager.World.Objects.Add(new Character());

        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
