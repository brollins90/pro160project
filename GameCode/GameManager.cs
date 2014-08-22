using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using GameCode.Models;
using System.Windows;
using GameCode.Helpers;
using GameCode.Models.Projectiles;

namespace GameCode
{
    public class GameManager
    {
        private DispatcherTimer Timer;
        private GameWorld _World;
        public GameWorld World
        {
            get { return _World; }
            set { _World = value; }
        }

        private ObservableCollection<Controller> _Controllers;
        public ObservableCollection<Controller> Controllers
        {
            get { return _Controllers; }
            set { _Controllers = value; }
        }

        private int LastTimeMillis = 0;
        

        public GameManager()
        {
            Timer = new DispatcherTimer();
            World = new GameWorld();
            Controllers = new ObservableCollection<Controller>();
            Timer.Interval = TimeSpan.FromMilliseconds((5));
            Timer.Start();
            Timer.Tick += Timer_Tick;

            LastTimeMillis = GetCurrentTime();
            LoadWorld("");

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            int currentTimeMillis = Environment.TickCount;
            int elapsedTimeMillis = currentTimeMillis - LastTimeMillis;
            Console.WriteLine("lastTime...: " + LastTimeMillis);
            Console.WriteLine("CurrentTime: " + currentTimeMillis);
            Console.WriteLine("elapsedTime: " + elapsedTimeMillis);
            float elapsedTimeFloat = (float)elapsedTimeMillis / 1000;
            Console.WriteLine(elapsedTimeFloat);

            Update(elapsedTimeFloat);
            LastTimeMillis = currentTimeMillis;
        }

        //public void AddNPC()
        //{
        //    GameObject newCharacter = new Bot(new Vector3(300, 150,0), this);
        //    World.Objects.Add(newCharacter);
        //}

        public void AddNPC(GameObject o)
        {
            World.Objects.Add(o);
        }

        public int AddPlayer(Controller playerController)
        {
            Character c = new Character(new Vector3(920, 800, 0), this);
            c.Controller = playerController;
            World.Objects.Add(c);
            Controllers.Add(playerController);
            return c.ID;
        }

        public void AddDebris(Debris debris)
        {
            World.Objects.Add(debris);
        }

        public void AddProjectile(GameProjectile p)
        {
            World.Objects.Add(p);            
        }

        public void LoadWorld(string filename)
        {
            AddNPC(new Bot(new Vector3(900, 100,0),this, BotClass.Boss));
            AddNPC(new Bot(new Vector3(750, 100, 0), this, BotClass.Melee));
            AddNPC(new Bot(new Vector3(910, 500, 0), this, BotClass.Mercenary));
            AddNPC(new Bot(new Vector3(800, 400, 0), this, BotClass.Shooter));
            AddNPC(new Bot(new Vector3(910, 300, 0), this, BotClass.Turret));
            AddNPC(new Bot(new Vector3(900, 30, 0), this, BotClass.Tower));
            AddDebris(new Debris(new Vector3(650, 880, 0), this, new Vector3(40, 200, 0)));
            AddDebris(new Debris(new Vector3(1250, 880, 0), this, new Vector3(40, 200, 0)));
            AddDebris(new Debris(new Vector3(650, 880, 0), this, new Vector3(250, 40, 0)));
            AddDebris(new Debris(new Vector3(1000, 880, 0), this, new Vector3(250, 40, 0)));
            AddDebris(new Debris(new Vector3(650, -10, 0), this, new Vector3(40, 200, 0)));
            AddDebris(new Debris(new Vector3(1250, -10, 0), this, new Vector3(40, 200, 0)));
            AddDebris(new Debris(new Vector3(650, 180, 0), this, new Vector3(250, 40, 0)));
            AddDebris(new Debris(new Vector3(1000, 180, 0), this, new Vector3(250, 40, 0)));
        }

        public void Update(float deltaTime)
        {
            Console.WriteLine("deltatime: " + deltaTime);
            
            // If paused, return
            // TODO

            // Update Pathing
            // TODO

            // Update Projectiles
            foreach (GameProjectile o in World.Projectiles)
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            // Update Bots
            foreach (Bot o in World.Bots) // implici cast
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            // Update the Players
            foreach (Character o in World.Characters) // implici cast
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }

            foreach (Debris o in World.Debris) 
            {
                if (o.Alive)
                {
                    o.Update(deltaTime);
                }
            }


            // Remove the dead
            World.Remove();

        }

        public void ResetGame()
        {

        }

        public void SendState()
        {

        }

        public void StartGame()
        {
        }

        public void SubmitMove(GameCommand command)
        {
            foreach (Controller c in Controllers)
            {
                if (c.GameObjectID == command.GameObjectId)
                {
                    c.KeyDown(command);
                }
            }
        }

        public int GetCurrentTime()
        {
            return Environment.TickCount;
        }
    }
}
