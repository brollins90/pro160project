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


        

        public GameManager()
        {
            Timer = new DispatcherTimer();
            World = new GameWorld();
            Controllers = new ObservableCollection<Controller>();
            Timer.Interval = TimeSpan.FromMilliseconds((100 / 1));
            Timer.Start();
            Timer.Tick += Timer_Tick;

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Update();
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

        public void AddProjectile(GameProjectile p)
        {
            World.Objects.Add(p);            
        }

        public void LoadWorld(string filename)
        {

        }

        public void Update()
        {
            // If paused, return
            // TODO

            // Update Pathing
            // TODO

            // Update Projectiles
            foreach (GameProjectile o in World.Projectiles)
            {
                if (o.Alive)
                {
                    o.Update();
                }
            }

            // Update Bots
            foreach (Bot o in World.Bots) // implici cast
            {
                if (o.Alive)
                {
                    o.Update();
                }
            }

            // Update the Players
            foreach (Character o in World.Characters) // implici cast
            {
                if (o.Alive)
                {
                    o.Update();
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
            //double lastTime = getCurrentTime();
        }

        public void SubmitMove()
        {

        }


        //public void SubmitMove(int GameObjectID, GameCommands keyPressed)
        //{
        //    Console.WriteLine("Manager.SubmitMove()");
        //    GameObject objToProcess = World.Objects.First(X => X.UniqueID == GameObjectID);
        //    Vector currentPosition = objToProcess.Position;
        //    Vector newPosition = currentPosition;

        //    if (keyPressed == GameCommands.Up) {
        //        newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y - objToProcess.Speed };
        //        objToProcess.Direction = 90;
        //    }
        //    else if (keyPressed == GameCommands.Down) {
        //        newPosition = new Vector() { X = currentPosition.X, Y = currentPosition.Y + objToProcess.Speed };
        //        objToProcess.Direction = 270;
        //    }
        //    else if (keyPressed == GameCommands.Left) {
        //        newPosition = new Vector() { X = currentPosition.X - objToProcess.Speed, Y = currentPosition.Y };
        //        objToProcess.Direction = 180;
        //    }
        //    else if (keyPressed == GameCommands.Right)
        //    {
        //        newPosition = new Vector() { X = currentPosition.X + objToProcess.Speed, Y = currentPosition.Y };
        //        objToProcess.Direction = 0;
        //    }
        //    else if (keyPressed == GameCommands.Space)
        //    {
        //        Console.WriteLine("recieved a space");
        //        //if (objToProcess.AttackType == AttackType.Ranged)
        //        //{
        //            AddProjectile(new GameProjectile(currentPosition, objToProcess.Direction, 25, objToProcess.Damage)
        //            {
        //                Height = 10,
        //                Width = 10,
        //                Controller = new Controller()

        //            });
        //        //}
        //    }
        //    objToProcess.Position = newPosition;

        //    bool collided = false;
        //    foreach (GameObject o in World.Objects)
        //    {
        //        if (objToProcess.UniqueID != o.UniqueID && objToProcess.CollidesWith(o))
        //            collided = true;
        //    }
        //    if (collided)
        //    {
        //        objToProcess.Position = currentPosition;
        //        Console.WriteLine("Collided");
        //    }


        //}
    }
}
