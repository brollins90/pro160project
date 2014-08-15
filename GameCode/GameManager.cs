using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using GameCode.Models;
using System.Drawing;

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
            Timer.Interval = TimeSpan.FromMilliseconds((1000 / 1));
            Timer.Start();
            Timer.Tick += Timer_Tick;

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            ProcessMoves();
        }

        public void AddNPC()
        {
            GameObject newCharacter = new Sentry(new Point(300, 150));
            Controller newController = new Controller()
            {
                GameObjectID = newCharacter.UniqueID
            };
            newCharacter.Controller = newController;
            World.Objects.Add(newCharacter);
            Controllers.Add(newController);


        }

        public int AddPlayer(Controller playerController)
        {
            Character c = new Character(new Point(100,100));
            c.Controller = playerController;
            World.Objects.Add(c);
            Controllers.Add(playerController);
            return c.UniqueID;

        }

        public void LoadWorld(string filename)
        {

        }

        public void ProcessMoves()
        {
            foreach (GameObject o in World.Sentrys)
            {
                SubmitMove(o.UniqueID, o.Controller.GetMove());
            }
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


        public void SubmitMove(int GameObjectID, GameCommands keyPressed)
        {
            Console.WriteLine("Manager.SubmitMove()");
            GameObject objToProcess = World.Objects.First(X => X.UniqueID == GameObjectID);
            Point currentPosition = objToProcess.Position;
            Point newPosition = currentPosition;

            if (keyPressed == GameCommands.Up) {
                newPosition = new Point() { X = currentPosition.X, Y = currentPosition.Y - objToProcess.Speed };
                objToProcess.Direction = 90;
            }
            else if (keyPressed == GameCommands.Down) {
                newPosition = new Point() { X = currentPosition.X, Y = currentPosition.Y + objToProcess.Speed };
                objToProcess.Direction = 270;
            }
            else if (keyPressed == GameCommands.Left) {
                newPosition = currentPosition;// new Point() { X = currentPosition.X - objToProcess.Speed, Y = currentPosition.Y };
                objToProcess.Direction += objToProcess.Speed;
            }
            else if (keyPressed == GameCommands.Right)
            {
                newPosition = currentPosition;// new Point() { X = currentPosition.X - objToProcess.Speed, Y = currentPosition.Y };
                objToProcess.Direction -= objToProcess.Speed;
            }
            objToProcess.Position = newPosition;

            bool collided = false;
            foreach (GameObject o in World.Objects)
            {
                if (objToProcess.UniqueID != o.UniqueID && objToProcess.CollidesWith(o))
                    collided = true;
            }
            if (collided)
            {
                objToProcess.Position = currentPosition;
                Console.WriteLine("Collided");
            }


        }
    }
}
