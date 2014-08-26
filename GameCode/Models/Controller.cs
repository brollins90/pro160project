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
        public InputListener InputListener { get; set; }
        public int ControllerID
        {
            get { return _ControllerID; }
            private set { _ControllerID = value; }
        }

        private int _GameObjectID;
        public int GameObjectID
        {
            get { return _GameObjectID; }
            set { _GameObjectID = value; }
        }

        //private GameCommands _Command;
        //public GameCommands Command
        //{
        //    get { return _Command; }
        //    set { _Command = value; }
        //}

        private GameCommand _Cmd;

        public GameCommand Cmd
        {
            get { return _Cmd; }
            set { _Cmd = value; }
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

        public void CreateCharacter(CharacterClasses type = Models.CharacterClasses.Fighter)
        {
            GameObjectID = Manager.AddPlayer(this, type);
            CurrentCharacter = Manager.World.Objects.First(c => { return c.ID == GameObjectID; }) as Character;
        }

        public Controller()
        {
            ControllerID = NextID++;
            Cmd = new GameCommand();
            Cmd.Command = GameCommands.None;
            InputListener = new InputListener();
        }

        public GameCommand GetMove()
        {
            GameCommand temp = Cmd.Copy();
            //Console.WriteLine("getmove: " + Command);
            Cmd.Command = GameCommands.None;
            return temp;
        }

        public void KeyDown(GameCommand cmd)
        {
            //Console.WriteLine("Controller.KeyDown()");
            Cmd = cmd;
            //Manager.SubmitMove(GameObjectID, keyPressed);
        }

    }
}
