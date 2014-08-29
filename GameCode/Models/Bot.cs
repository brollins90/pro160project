using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GameCode.Models.Weapons;
using GameCode.Helpers;
using GameCode.Models.Projectiles;
using System.Timers;

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
                this.FirePropertyChanged("BotWeapon"); 
            }
        }

        public Bot(Vector3 position, GameManager manager, BotClass type = Models.BotClass.Melee)
            : base(position, manager, new Vector3(0,0,0))
        {

            switch (type)
            {
                case Models.BotClass.Boss:
                    this.Acceleration = new Vector3(1, 1, 0);
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 25;
                    this.Health = 1000;
                    this.MaxHealth = Health;
                    Size = new Vector3(50,50,0);
                    break;
                case Models.BotClass.Melee:
                    this.Acceleration = new Vector3(3, 3, 0);
                    this.AttackRadiusSquared = 1000 * 1000;
                    this.BotClass = type;
                    this.BotWeapon = new Sword(this);
                    this.Damage = 9;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    Size = new Vector3(20,20,0);
                    break;
                case Models.BotClass.Mercenary: // Sentry
                    this.Acceleration = new Vector3(2, 2, 0);
                    this.AttackRadiusSquared = 100 * 100;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 13;
                    this.Health = 500;
                    this.MaxHealth = Health;
                    Size = new Vector3(30,30,0);
                    break;
                case Models.BotClass.Shooter: // ???
                    this.Acceleration = new Vector3(1, 1, 0);
                    this.AttackRadiusSquared = 1000 * 1000;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 7;
                    this.Health = 10;
                    this.MaxHealth = Health;
                    Size = new Vector3(20, 20,0);
                    break;
                case Models.BotClass.Tower: // Need to kill this to win
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 1 * 1;
                    this.BotClass = type;
                    this.BotWeapon = new CrossBow(this);
                    this.Damage = 0;
                    this.Health = 1500;
                    this.MaxHealth = Health;
                    Size = new Vector3(100,100,0);
                    break;
                case Models.BotClass.Turret: // stationary
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 200 * 200;
                    this.BotClass = type;
                    this.BotWeapon = new Magic(this);
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


        public virtual void CheckInput(double deltaTime)
        {
            double closestLengthSquared = double.MaxValue;
            Bot closestEnemy = null;

            //Console.WriteLine("Bot: " + this.ID);
            foreach (Bot b in Manager.World.Enemies(this.Team))
            {
                //Console.WriteLine("Enemy: " + b.ID);
                double distanceFromSquared = (Position - b.Position).LengthSquared();
                //Console.WriteLine("distance: " + distanceFromSquared);
                if (distanceFromSquared < AttackRadiusSquared && distanceFromSquared < closestLengthSquared)
                {
                    closestEnemy = b;
                    //target = b.Position;
                    closestLengthSquared = distanceFromSquared;
                }
            
            }

            if (closestEnemy != null)
            {
                if (RotateTowardPosition(closestEnemy.Position))
                {
                    if (closestLengthSquared > BotWeapon.ProjectileRangeSquared)
                    {
                        // get closer
                        MoveForward(deltaTime);
                    }
                    else
                    {
                        BotWeapon.Attack();
                    }
                }
            }
        }

        public override void Update(double deltaTime)
        {
            CheckInput(deltaTime);

            Vector3 previousPosition = new Vector3(Position.x, Position.y, Position.z);

            base.Update(deltaTime);

            // check for new collisions
            bool collided = false;
            foreach (GameObject o in Manager.World.Collidables)
            {
                if (this.ID != o.ID && this.CollidesWith(o))
                    if (o.GetType() == typeof(GameProjectile) && ((GameProjectile)o).Owner.ID == this.ID)
                    {
                        // do nothing
                    }
                    else
                    {
                        collided = true;
                    }
            }
            // if collided dont perform the move
            if (collided)
            {
                this.Position = previousPosition;
            }

        }

        public void HasDied()
        {
            Alive = false;
        }

        public void TakeDamage(int amount)
        {
            //Console.WriteLine("TakeDamage() " + amount);
            DecreaseHealth(amount);
            //Console.WriteLine("Health after: " + Health);
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

