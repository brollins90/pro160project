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

        private double _AttackRadiusSquared;
        public double AttackRadiusSquared
        {
            get { return _AttackRadiusSquared; }
            set { _AttackRadiusSquared = value;
            this.FirePropertyChanged("AttackRadiusSquared");
            }
        }

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
                this.FirePropertyChanged("BotWeapon"); }
        }

        public Bot(Vector3 position, GameManager manager, BotClass type = Models.BotClass.Melee)
            : base(position, manager, new Vector3(0,0,0))
        {
            Team = GameManager.TEAM_INT_BADDIES;

            switch (type)
            {
                case Models.BotClass.Boss:
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 25;
                    this.Health = 1000;
                    this.MaxHealth = Health;
                    Size = new Vector3(50,50,0);
                    break;
                case Models.BotClass.Melee:
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 9;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    Size = new Vector3(20,20,0);
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    this.AttackRadiusSquared = 100 * 100;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 13;
                    this.Health = 500;
                    this.MaxHealth = Health;
                    Size = new Vector3(30,30,0);
                    break;
                case Models.BotClass.Shooter: // ???
                    this.AttackRadiusSquared = 400 * 400;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 7;
                    this.Health = 10;
                    this.MaxHealth = Health;
                    Size = new Vector3(20, 20,0);
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 0;
                    this.Health = 1500;
                    this.MaxHealth = Health;
                    Size = new Vector3(100,100,0);
                    break;
                case Models.BotClass.Turret: // stationary
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 16;
                    this.Health = 750;
                    this.MaxHealth = Health;
                    Size = new Vector3(60,60,0);
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

        public override void Update(double deltaTime)
        {
            Vector3 target = new Vector3();
            double closestLengthSquared = double.MaxValue;

            //Console.WriteLine("Bot: " + this.ID);
            foreach (Bot b in Manager.World.Enemies(this.Team))
            {
                //Console.WriteLine("Enemy: " + b.ID);
                double distanceFromSquared = (Position - b.Position).LengthSquared();
                //Console.WriteLine("distance: " + distanceFromSquared);
                if (distanceFromSquared < AttackRadiusSquared && distanceFromSquared < closestLengthSquared)
                {
                    target = b.Position;
                    closestLengthSquared = distanceFromSquared;
                }
            }

            if (!target.IsZero())
            {
                RotateTowardPosition(target);
                BotWeapon.Attack();
            }
        }

        public void HasDied()
        {
            Alive = false;
        }

        public void TakeDamage(int amount)
        {
            Console.WriteLine("TakeDamage() " + amount);
            DecreaseHealth(amount);
            Console.WriteLine("Health after: " + Health);
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

