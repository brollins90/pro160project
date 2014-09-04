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
        private MTObservableCollection<GameObject> _Objects;
        public MTObservableCollection<GameObject> Objects
        {
            get { return _Objects; }
            set { _Objects = value; }
        }

        public GameWorld()
        {
            Objects = new MTObservableCollection<GameObject>();
        }

        public void AddObject(GameObject o)
        {
            //Console.WriteLine("{0} GameWorld - Add: {1}, {2}", System.Threading.Thread.CurrentThread.ManagedThreadId, o.ID, o.ClassType);
            lock (Objects)
            {
                Objects.Add(o);
            }
        }

        //public bool Contains(int id)
        //{
        //    return Get(id) != null;
        //}

        public GameObject Get(int id)
        {
            //Console.WriteLine("{0} GameWorld - get: {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, id);
            GameObject o = null;
            try
            {
                lock (Objects)
                {
                    o = Objects.FirstOrDefault((obj) => obj.ID == id);
                    //o = Objects.FirstOrDefault((obj) => obj.ID == id);
                }
            }
            //catch (InvalidOperationException ex)
            //{
            //    //Console.WriteLine("OBJ doesnt exist: {0}", ex.ToString());
            //}
            catch (Exception)
            {

            }
            return o;
        }

        public void RemoveObject(int objectID)
        {
            RemoveObject(Get(objectID));
        }

        public void RemoveObject(GameObject o)
        {
            //Console.WriteLine("{0} GameWorld - remove: {1}, {2}", System.Threading.Thread.CurrentThread.ManagedThreadId, o.ID, o.ClassType);
            lock (Objects)
            {
                Objects.Remove(o);
            }
        }

        public MTObservableCollection<GameObject> Alive
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive); }).ToList();
                return new MTObservableCollection<GameObject>(retVal);
            }
        }

        public MTObservableCollection<GameObject> Bots
        {
            get
            {
                var retVal = Objects.Where((obj, r) => { return (obj.Alive && obj.GetType() == typeof(Bot)); }).ToList();
                return new MTObservableCollection<GameObject>(retVal);
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

        public List<GameObject> Dead
        {
            get
            {
                var retVal = Objects.Where(obj => obj.Alive != true).ToList();
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
    }
}
