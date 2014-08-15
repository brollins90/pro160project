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

        public int _Team;
        public int Team
        {
            get { return _Team; }
            set { _Team = value; }
        }

        private int _AttackDamage;
        public int AttackDamage
        {
            get { return _AttackDamage; }
            set { _AttackDamage = value; }
        }

        public Bot(int speed, int health, int attackDamage, int team, Point position) : base(position)
        {
            this._AttackDamage = attackDamage;
            this.Speed = speed;
            this._Team = team;
            this.Health = health;
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

