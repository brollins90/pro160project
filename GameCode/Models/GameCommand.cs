using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public enum GameCommands
    {
        None, Up, Left, Down, Right, Space, MouseMove, LeftClick, RightClick
    }
    public class GameCommand
    {
        public int GameObjectId { get; set; }
        public GameCommands Command { get; set; }
        public float CommandTime { get; set; }
        public object Additional { get; set; }

        public GameCommand()
        {

        }
        public GameCommand(int gameObjectId, GameCommands command, float commandTime)
        {
            GameObjectId = gameObjectId;
            Command = command;
            CommandTime = commandTime;
        }

        public GameCommand(int gameObjectId, GameCommands command, float commandTime, object additional)
        {
            GameObjectId = gameObjectId;
            Command = command;
            CommandTime = commandTime;
            Additional = additional;
        }

        public GameCommand Copy()
        {
            GameCommand temp = new GameCommand(GameObjectId,Command,CommandTime);
            temp.Additional = Additional;
            return temp;
        }

    }
}
