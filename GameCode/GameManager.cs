using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using GameCode.Models;
using System.Windows;

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

        private int lastTime = 0;
        

        public GameManager()
        {
            Timer = new DispatcherTimer();
            World = new GameWorld();
            Controllers = new ObservableCollection<Controller>();
            Timer.Interval = TimeSpan.FromMilliseconds((16));
            Timer.Start();
            Timer.Tick += Timer_Tick;

            lastTime = GetCurrentTime();

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            int currentTime = Environment.TickCount;
            int elapsedTime = currentTime - lastTime;
            Update(elapsedTime);
            lastTime = currentTime;
        }

        public void AddNPC()
        {
            GameObject newCharacter = new Bot(new Vector(300, 150), this);
            //GameObject newCharacter = new Bot(new Point(300, 150));
            //Controller newController = new BotController()
            //{
            //    GameObjectID = newCharacter.UniqueID
            //};
            //newCharacter.Controller = newController;
            World.Objects.Add(newCharacter);
            //Controllers.Add(newController);


        }

        public int AddPlayer(Controller playerController)
        {
            Character c = new Character(new Vector(100, 100), this);
            c.Controller = playerController;
            World.Objects.Add(c);
            Controllers.Add(playerController);
            return c.UniqueID;

        }

        public void AddDebris()
        {
            Debris debris = new Debris(new Vector(400, 500), this, 30, 100);
            World.Objects.Add(debris);
        }

        public void AddProjectile(GameProjectile p)
        {
            World.Objects.Add(p);            
        }

        public void LoadWorld(string filename)
        {

        }

        public void Update(int deltaTime)
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

        public void SubmitMove()
        {

        }

        public int GetCurrentTime()
        {
            return Environment.TickCount;
        }
    }
}
