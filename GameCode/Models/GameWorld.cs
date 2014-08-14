using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public class GameWorld
    {
        private ObservableCollection<GameObject> _Objects;

        public ObservableCollection<GameObject> Objects
        {
            get { return _Objects; }
            set { _Objects = value; }
        }

        public GameWorld()
        {
            Objects = new ObservableCollection<GameObject>();
        }
        
    }
}
