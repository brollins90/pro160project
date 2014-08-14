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
        private int _ControllerID;

        public int ControllerID
        {
            get { return _ControllerID; }
            set { _ControllerID = value; }
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
        }

        public Controller()
        {
            ControllerID = 7;

        }

        public void KeyDown(GameCommands keyPressed)
        {
            Console.WriteLine("Controller.KeyDown()");
            Manager.SubmitMove(GameObjectID, keyPressed);
        }
    }
}
