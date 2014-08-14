using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCode.Models;
using System.Drawing;

namespace GameCode
{
    public class GameManager
    {
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
            World = new GameWorld();

        }

        public int AddPlayer(Controller playerController)
        {
            Character c = new Character();
            World.Objects.Add(c);
            return c.UniqueID;

        }

        public void LoadWorld(string filename)
        {

        }

        public void ProcessMoves()
        {

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
                newPosition = new Point() { X = currentPosition.X - objToProcess.Speed, Y = currentPosition.Y};
                objToProcess.Direction = 180;
            }
            else if (keyPressed == GameCommands.Right)
            {
                newPosition = new Point() { X = currentPosition.X + objToProcess.Speed, Y = currentPosition.Y };
                objToProcess.Direction = 0;
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
