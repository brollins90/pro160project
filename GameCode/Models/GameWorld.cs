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
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(Bot); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Projectiles
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(GameProjectile); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Characters
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(Character); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Debris
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(Debris); }).ToList();
                return retVal;
            }
        }


        internal void Remove()
        {
            var toRemove = Objects.Where(obj => obj.Alive != true).ToList();
            foreach (var item in toRemove)
            {
                Objects.Remove(item);
            }
        }
    }
}
