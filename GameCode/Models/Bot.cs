using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{
    public enum BotClass { Shooter, Melee, Boss, Tower, Turret, Mercenary, Player };

    //The main class for all Non Playable Characters. The information on them is based off of the information passed through the constructor.
    public class Bot : MovingObject
    {
        private BotClass _BotClass;
        public BotClass BotClass
        {
            get { return _BotClass; }
            set
            {
                _BotClass = value;
                this.FirePropertyChanged("BotClass");
        }
        }


        private AttackType _AttackType;
        public AttackType AttackType
        {
            get { return _AttackType; }
            set
            {
                _AttackType = value;
                this.FirePropertyChanged("AttackType");
        }
        }

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set
            {
                _Damage = value;
                this.FirePropertyChanged("Damage");
            }
        }


        private int _Health;
        public int Health
        {
            get { return _Health; }
            set
            {
                _Health = value;
                this.FirePropertyChanged("Health");
            }
        }
        private int _MaxHealth;
        public int MaxHealth
        {
            get { return _MaxHealth; }
            set
            {
                _MaxHealth = value;
                this.FirePropertyChanged("MaxHealth");
            }
        }

        //public Bot(int speed, int health, int attackDamage, int team, Vector position, GameManager manager)
        //    : base(position, manager)
        //{
        //    this.Speed = speed;
        //    this._Team = team;
        //    this.Health = health;
        //}

        public Bot(Vector position, GameManager manager, BotClass type = Models.BotClass.Melee)
            : base(position, manager, new Vector(0,0))
        {
            switch (type)
            {
                case Models.BotClass.Boss:
                    //this.Controller;
                    //this.MoveType;
                    //this.UniqueID;
                    this.Speed = 1;
                    this.Health = 100;
                    this.MaxHealth = Health;
                    Size = new Vector(50,50);
                    this.Damage = 40;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Melee:
                    //this.Speed = 3;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    Size = new Vector(20,20);
                    this.Damage = 20;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    //this.Speed = 1;
                    this.Health = 50;
                    this.MaxHealth = Health;
                    Size = new Vector(30,30);
                    this.Damage = 30;
                    this.AttackType = Melee;
                    break;
                case Models.BotClass.Shooter: // ???
                    //this.Speed = 2;
                    this.Health = 10;
                    this.MaxHealth = Health;
                    Size = new Vector(20,20);
                    this.Damage = 15;
                    this.AttackType = Ranged;
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    //this.Speed = 0;
                    this.Health = 75;
                    this.MaxHealth = Health;
                    Size = new Vector(75,75);
                    this.Damage = 0;
                    break;
                case Models.BotClass.Turret: // stationary
                    //this.Speed = 0;
                    this.Health = 75;
                    this.MaxHealth = Health;
                    Size = new Vector(60,60);
                    this.Damage = 50;
                    this.AttackType = Ranged;
                    break;
            }
        }
        public void ReduceHealth(int val)
        {
            Health -= val;
        }
        public void IncreaseHealth(int val)
        {
            Health += val;
        }

        public void Attack(Vector destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Vector destination)
        {
            throw new NotImplementedException();
        }

        //private int temp = 0;
        public override void Update(float deltaTime)
            {
            Position = Position + Velocity * deltaTime;
            //temp++;
            //if (temp < 50) { 
            //    Position = new Vector(Position.X + Speed, Position.Y + Speed);
            //}
            //else if (temp < 100)
            //{
            //    Position = new Vector(Position.X - Speed, Position.Y + Speed);
            //}
            //else if (temp < 150)
            //{
            //    Position = new Vector(Position.X - Speed, Position.Y - Speed);
            //}
            //else if (temp < 200)
            //{
            //    Position = new Vector(Position.X + Speed, Position.Y - Speed);
            //}
            //else
            //{
            //    temp = 0;
            //}
        }

        public Models.AttackType Melee { get; set; }

        public Models.AttackType Ranged { get; set; }

    }
}

