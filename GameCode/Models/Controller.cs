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

        public Character CurrentCharacter { get; set; }

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

        public GameCommands Command;

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
            CurrentCharacter = Manager.World.Objects.First(c => { return c.UniqueID == GameObjectID; }) as Character;
        }

        public Controller()
        {
            ControllerID = NextID++;
            Command = GameCommands.None;
        }

        public GameCommands GetMove()
        {
            Console.WriteLine("getmove: " + Command);
            GameCommands current = Command;
            Command = GameCommands.None;
            return current;
        }

        public void KeyDown(GameCommands keyPressed)
        {
            Console.WriteLine("Controller.KeyDown()");
            Command = keyPressed;
            //Manager.SubmitMove(GameObjectID, keyPressed);
        }

    }
}
