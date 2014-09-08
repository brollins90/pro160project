using System;
using System.Collections.Generic;
using System.Linq;
using GameCode.Models.Projectiles;

namespace GameCode.Models
{
    /// <summary>
    /// The basic model for holding the game objects
    /// </summary>
    public class GameWorld
    {
        /// <summary>
        /// The primary collection for the objects.  Since we are using WPF, 
        /// we have to make it observable... Since WPF sucks,
        /// we have to manually make it thread safe
        /// </summary>
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

        /// <summary>
        /// Adds an object to the world
        /// </summary>
        /// <param name="o"></param>
        public void AddObject(GameObject o)
        {
            lock (Objects)
            {
                Objects.Add(o);
            }
        }

        /// <summary>
        /// Retreives an object from the world
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject Get(int id)
        {
            GameObject o = null;
            try
            {
                lock (Objects)
                {
                    o = Objects.FirstOrDefault((obj) => obj.ID == id);
                }
            }
            catch (Exception)
            {
                // catch and do nothing
            }
            return o;
        }

        /// <summary>
        /// Remove the object with specified ID from the world
        /// </summary>
        /// <param name="objectID"></param>
        public void RemoveObject(int objectID)
        {
            RemoveObject(Get(objectID));
        }

        /// <summary>
        /// Removes the object from the world
        /// </summary>
        /// <param name="o"></param>
        public void RemoveObject(GameObject o)
        {
            lock (Objects)
            {
                Objects.Remove(o);
            }
        }

        /// <summary>
        /// Returns all the bots
        /// </summary>
        public IEnumerable<GameObject> Bots
        {
            get
            {
                return Objects.Where(x => (x is Bot && x.Alive)).ToList();
            }
        }

        /// <summary>
        /// Returns all the Characters
        /// </summary>
        public IEnumerable<GameObject> Characters
        {
            get
            {
                return Objects.Where(x => (x is Character && x.Alive)).ToList();
            }
        }

        /// <summary>
        /// Returns anything not alive
        /// </summary>
        public IEnumerable<GameObject> Dead
        {
            get
            {
                return Objects.Where(x => (x.Alive == false)).ToList();
            }
        }

        /// <summary>
        /// Returns all the debris
        /// </summary>
        public IEnumerable<GameObject> Debris
        {
            get
            {
                return Objects.Where(x => (x is Debris && x.Alive)).ToList();
            }
        }

        /// <summary>
        /// Returns all the enemy bots
        /// </summary>
        /// <param name="currentTeam"></param>
        /// <returns></returns>
        public IEnumerable<GameObject> Enemies(int currentTeam)
        {
            return Objects.Where(x => (x is Bot && x.Alive && x.Team != currentTeam)).ToList();
        }

        /// <summary>
        /// Returns anything that is alive and not a projectile
        /// </summary>
        public IEnumerable<GameObject> NotProjectiles
        {
            get
            {
                return Objects.Where(x => (!(x is GameProjectile) && x.Alive)).ToList();
            }
        }

        /// <summary>
        /// Returns any projectiles
        /// </summary>
        public IEnumerable<GameObject> Projectiles
        {
            get
            {
                return Objects.Where(x => (x is GameProjectile && x.Alive)).ToList();
            }
        }
    }
}
