using GameCode.Models.Projectiles;
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

        public List<GameObject> Characters
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(Character); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Collidables
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.GetType() != typeof(GameProjectile) && obj.GetType() != typeof(Arrow) && obj.GetType() != typeof(StabAttack) && obj.GetType() != typeof(FireBall)); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> CastleWalls
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(CastleWalls); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Bushes
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return obj.GetType() == typeof(Bushes); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Enemies(int currentTeam)
        {            
            var retVal = Objects.Where((obj, r) => { return ((obj.GetType() == typeof(Bot) || obj.GetType() == typeof(Character)) && obj.Team != currentTeam ); }).ToList();
            return retVal;           
        }

        public List<GameObject> Projectiles
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.GetType() == typeof(GameProjectile) || obj.GetType() == typeof(Arrow) || obj.GetType() == typeof(StabAttack) || obj.GetType() == typeof(FireBall)); }).ToList();
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
