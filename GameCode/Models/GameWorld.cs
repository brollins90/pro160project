using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

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

        public List<GameObject> Bots
        {
            get { 
                var bots = Objects.Where((obj, r) => { return obj.GetType() == typeof(Bot); }).ToList();

                //var sentrys = CollectionViewSource.GetDefaultView(Objects);
                //sentrys.Filter = s => (s as Sentry).GetType() == typeof(Sentry);
                return bots;
            }
        }
        
    }
}
