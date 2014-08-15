using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    //The main class for all Non Playable Characters
    public class Bot : GameObject, IMovingObject, IAttackingObject
    {
        public enum BotClass { Shooter, Melee, Boss, Tower, Turret, Mercenary };

        public int _Speed;
        public int Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        public int _Team;
        public int Team
        {
            get { return _Team; }
            set { _Team = value; }
        }

        private int _Health;
        public int Health
        {
            get { return _Health; }
            set { _Health = value; }
        }

        private int _Attack;
        public int Attack
        {
            get { return _Attack; }
            set { _Attack = value; }
        }

        public Bot(int speed, int health, int attack, int team, Point position) : base(position)
        {
            this._Attack = attack;
            this._Speed = speed;
            this._Team = team;
            this._Health = health;
        }

        public void Attack(Point destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Point destination)
        {
            throw new NotImplementedException();
        }
    }
}

