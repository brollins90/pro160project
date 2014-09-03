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

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value;
                this.FirePropertyChanged("Damage");
        }
        }



        private int _Health;
        public int Health
        {
            get { return _Health; }
            set { _Health = value;
                this.FirePropertyChanged("Health");
            }
        }

        private int _MaxHealth;
        public int MaxHealth
        {
            get { return _MaxHealth; }
            set { _MaxHealth = value;
                this.FirePropertyChanged("MaxHealth");
            }
        }

        private int _ExpYield;
        public int ExpYield
            {
            get { return _ExpYield; }
            set { _ExpYield = value;
            this.FirePropertyChanged("ExpYield");
            }
        }

        private Weapon _Weapon;
        public Weapon Weapon
        {
            get { return _Weapon; }
            set { _Weapon = value;
                this.FirePropertyChanged("Weapon"); 
            }
        }

        public Bot(Vector3 position, GameManager manager, int type = GameConstants.TYPE_BOT_MELEE)
            : base(position, manager, new Vector3(0,0,0))
        {
            Team = GameConstants.TEAM_INT_BADDIES;

            switch (type)
            {
                case GameConstants.TYPE_BOT_BOSS:
                    this.Acceleration = new Vector3(1, 1, 0);
                    this.AttackRadiusSquared = 200 * 200;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 25;
                    this.Health = 1000;
                    this.MaxHealth = Health;
                    this.ExpYield = 250;
                    Size = new Vector3(50,50,0);
                    break;
                case GameConstants.TYPE_BOT_MELEE:
                    this.Acceleration = new Vector3(3, 3, 0);
                    this.AttackRadiusSquared = 2000 * 2000;
                    this.ClassType = type;
                    this.Weapon = new Sword(this);
                    this.Damage = 9;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    this.ExpYield = 15;
                    Size = new Vector3(20,20,0);
                    break;
                case GameConstants.TYPE_BOT_MERCENARY: // Sentry
                    this.Acceleration = new Vector3(2, 2, 0);
                    this.AttackRadiusSquared = 100 * 100;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 13;
                    this.Health = 500;
                    this.MaxHealth = Health;
                    this.ExpYield = 100;
                    Size = new Vector3(30,30,0);
                    break;
                case GameConstants.TYPE_BOT_SHOOTER: // ???
                    this.Acceleration = new Vector3(2, 2, 0);
                    this.AttackRadiusSquared = 400 * 400;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 7;
                    this.Health = 10;
                    this.MaxHealth = Health;
                    this.ExpYield = 10;
                    Size = new Vector3(20, 20,0);
                    break;
                case GameConstants.TYPE_BOT_TOWER: // Need to kill this to win
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 1 * 1;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 0;
                    this.Health = 1500;
                    this.MaxHealth = Health;
                    this.ExpYield = 5000;
                    Size = new Vector3(100,100,0);
                    break;
                case GameConstants.TYPE_BOT_TURRET: // stationary
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 200 * 200;
                    this.ClassType = type;
                    this.Weapon = new Magic(this);
                    this.Damage = 16;
                    this.Health = 750;
                    this.MaxHealth = Health;
                    this.ExpYield = 150;
                    Size = new Vector3(60,60,0);
                    break;

            }
        }
        protected void DecreaseHealth(int val)
        {
            Health -= val;
            if (Health <= 0)
            {
                Alive = false;
            }
        }

        protected void IncreaseHealth(int val)
        {
            Health += val;
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public virtual void TakeDamage(int amount, Bot attacker)
        {
            DecreaseHealth(amount);
            if (!Alive)
            {
                if (attacker.GetType() == typeof(Character))
                {
                    ((Character)attacker).IncreaseExperience(this.ClassType);
                }
            }
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
                    if (closestLengthSquared > Weapon.Projectile.RangeSquared)
                    {
                        // get closer
                        MoveForward(deltaTime);
                    }
                    else
                    {
                        Weapon.Attack();
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
    }
}

