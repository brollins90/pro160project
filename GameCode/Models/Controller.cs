using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public class Controller
    {

        public GameObject CurrentObject { get; set; }

        private static int NextID = 0;
        private int _ControllerID;
        public int ControllerID
        {
            get { return _ControllerID; }
            private set { _ControllerID = value; }
        }

        private int _GameObjectID;
	    public int GameObjectID
	    {
		    get { return _GameObjectID;}
		    set { _GameObjectID = value;}
	    }

        private GameManager _Manager;

        public GameManager Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }
        

        public int Connect(GameManager manager)
        {
            Manager = manager;
            return 0;
        }

        public void CreateCharacter()
        {
            GameObjectID = Manager.AddPlayer(this);
            CurrentObject = Manager.World.Objects.First(c => { return c.UniqueID == GameObjectID; }) as Character;
        }

        public Controller()
        {
            ControllerID = NextID++;
        }

        public GameCommands GetMove()
        {
            return GameCommands.None;
        }
        public void KeyDown(GameCommands keyPressed)
        {
            Console.WriteLine("Controller.KeyDown()");
            Manager.SubmitMove(GameObjectID, keyPressed);
        }

    }
}
