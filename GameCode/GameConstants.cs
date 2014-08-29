using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode
{
    public static class GameConstants
    {
        public const int MSG_ADD = 1;
        public const int MSG_UPDATE = 2;
        public const int MSG_DEAD = 3;
        public const int MSG_GAMEOVER = 4;
        // 8
        // 16
        // 32
        // 64
        public const int MOUSE_POS = 65;

        public const int MOVEMENT_UP = 128;
        public const int MOVEMENT_DOWN = 129;
        public const int MOVEMENT_LEFT = 130;
        public const int MOVEMENT_RIGHT = 131;
        public const int MOVEMENT_ATTACK = 132;

        public const int TYPE_PROJ_ARROW = 180;
        public const int TYPE_PROJ_STAB = 181;
        public const int TYPE_PROJ_FIRE = 182;

        public const int TYPE_BOT_BOSS = 200;
        public const int TYPE_BOT_MELEE = 201;
        public const int TYPE_BOT_MERCENARY = 202;
        public const int TYPE_BOT_SHOOTER = 203;
        public const int TYPE_BOT_TOWER = 204;
        public const int TYPE_BOT_TURRET = 205;

        public const int TYPE_DEBRIS_ROCK = 220;
        public const int TYPE_DEBRIS_BUSH = 221;
        public const int TYPE_DEBRIS_WALL = 222;

        public const int TYPE_CHARACTER_MAGE = 235;
        public const int TYPE_CHARACTER_FIGHTER = 236;
        public const int TYPE_CHARACTER_ARCHER = 237;
        
    }
}
