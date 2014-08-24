using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameCode.Models.Weapons;
using GameCode.Helpers;

namespace GameCode.Models
{
    public enum BotClass { 
        Boss,
        Melee,
        Mercenary,
        Shooter,
        Tower,
        Turret };

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

        private Weapon _BotWeapon;

        public Weapon BotWeapon
        {
            get { return _BotWeapon; }
            set { _BotWeapon = value;
                this.FirePropertyChanged("BotWeapon"); 
            }
        }

        public Bot(Vector3 position, GameManager manager, BotClass type = Models.BotClass.Melee)
            : base(position, manager, new Vector3(0,0,0))
        {
            switch (type)
            {
                case Models.BotClass.Boss:
                    this.BotClass = type;
                    //this.Controller;
                    //this.MoveType;
                    //this.UniqueID;
                    //this.Speed = 1;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 25;
                    this.Health = 1000;
                    this.MaxHealth = Health;
                    Size = new Vector3(50,50,0);
                    break;
                case Models.BotClass.Melee:
                    this.BotClass = type;
                    //this.Speed = 3;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 9;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    Size = new Vector3(20,20,0);
                    //this.AttackType = Melee;
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    this.BotClass = type;
                    //this.Speed = 1;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 13;
                    this.Health = 500;
                    this.MaxHealth = Health;
                    Size = new Vector3(30,30,0);
                    //this.AttackType = Melee;
                    break;
                case Models.BotClass.Shooter: // ???
                    this.BotClass = type;
                    //this.Speed = 2;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 7;
                    this.Health = 10;
                    this.MaxHealth = Health;
                    Size = new Vector3(20, 20,0);
                    //this.AttackType = Ranged;
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    this.BotClass = type;
                    //this.Speed = 0;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 0;
                    this.Health = 1500;
                    this.MaxHealth = Health;
                    Size = new Vector3(100,100,0);
                    break;
                case Models.BotClass.Turret: // stationary
                    this.BotClass = type;
                    //this.Speed = 0;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 16;
                    this.Health = 750;
                    this.MaxHealth = Health;
                    Size = new Vector3(60,60,0);
                    //this.AttackType = Ranged;
                    break;
            }
        }
        protected void DecreaseHealth(int val)
        {
            Health -= val;
        }
        protected void IncreaseHealth(int val)
        {
            Health += val;
        }

        //public void Attack(Vector3 destination)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Move(Vector3 destination)
        //{
        //    throw new NotImplementedException();
        //}

        private int temp = 0;
        public override void Update(double deltaTime)
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

        public void HasDied()
        {
            Alive = false;
        }

        public void TakeDamage(int amount)
        {
            DecreaseHealth(amount);
            if (Health <= 0)
            {
                HasDied();
            }
        }

        public void GiveHealth(int amount)
        {
            IncreaseHealth(amount);
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
        }
    }
}

