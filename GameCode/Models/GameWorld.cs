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

        public void Add(GameObject o)
        {
            Objects.Add(o);
        }

        public GameObject Get(int id)
        {
            try
            {
                return Objects.First((obj) => obj.ID == id);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("OBJ doesnt exist: {0}", ex.ToString());
            }
            return null;
        }

        internal void RemoveDead()
        {
            var toRemove = Objects.Where(obj => obj.Alive != true).ToList();
            foreach (var item in toRemove)
            {
                Objects.Remove(item);
            }
        }

        public List<GameObject> Bots
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && obj.GetType() == typeof(Bot)); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Characters
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && obj.GetType() == typeof(Character)); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Collidables
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && obj.GetType() != typeof(GameProjectile) && obj.GetType() != typeof(Arrow) && obj.GetType() != typeof(StabAttack) && obj.GetType() != typeof(FireBall)); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Debris
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && obj.GetType() == typeof(Debris) && obj.GetType() == typeof(CastleWalls) && obj.GetType() == typeof(Bushes) && obj.GetType() == typeof(Rocks)); }).ToList();
                return retVal;
            }
        }

        public List<GameObject> Enemies(int currentTeam)
        {
            var retVal = Objects.Where((obj, r) => { return (obj.Alive && (obj.GetType() == typeof(Bot) || obj.GetType() == typeof(Character)) && obj.Team != currentTeam); }).ToList();
            return retVal;           
        }

        public List<GameObject> Projectiles
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && (obj.GetType() == typeof(GameProjectile) || obj.GetType() == typeof(Arrow) || obj.GetType() == typeof(StabAttack) || obj.GetType() == typeof(FireBall))); }).ToList();
                return retVal;
            }
        }

        public bool Contains(int id)
        {
            var retVal = Objects.Where((obj, r) => { return obj.ID == id; }).ToList();
            return retVal.Count == 1;
        }
    }
}
