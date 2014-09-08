using GameCode.Helpers;
using GameCode.Models.Projectiles;
using GameCode.Models.Weapons;

namespace GameCode.Models
{
    //The main class for all Actionable Characters.
    public class Bot : MovingObject
    {
        /// <summary>
        /// The distance required for the Bot to Attack
        /// </summary>
        private double _AttackRadiusSquared;
        public double AttackRadiusSquared
        {
            get { return _AttackRadiusSquared; }
            set
            {
                _AttackRadiusSquared = value;
            this.FirePropertyChanged("AttackRadiusSquared");
            }
        }

        /// <summary>
        /// The damage that one attack will cause
        /// </summary>
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

        /// <summary>
        /// The number of health points
        /// </summary>
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

        /// <summary>
        /// The max number of points the unit will have
        /// </summary>
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

        /// <summary>
        /// The weapon this bot uses
        /// </summary>
        private Weapon _Weapon;
        public Weapon Weapon
        {
            get { return _Weapon; }
            set
            {
                _Weapon = value;
                this.FirePropertyChanged("Weapon"); 
            }
        }


        public Bot(Vector3 position, GameManager manager, int type = GameConstants.TYPE_BOT_MELEE)
            : base(position, manager, new Vector3(0, 0, 0))
        {
            Angle = 90;
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
                    Size = new Vector3(50, 50, 0);
                    break;
                case GameConstants.TYPE_BOT_MELEE:
                    this.Acceleration = new Vector3(4, 4, 0);
                    this.AttackRadiusSquared = 2000 * 2000;  // Huge range to componsate for lack of AI
                    this.ClassType = type;
                    this.Weapon = new Sword(this);
                    this.Damage = 9;
                    this.Health = 25;
                    this.MaxHealth = Health;
                    Size = new Vector3(20, 20, 0);
                    break;
                case GameConstants.TYPE_BOT_MERCENARY: // Sentry
                    this.Acceleration = new Vector3(2, 2, 0);
                    this.AttackRadiusSquared = 100 * 100;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 13;
                    this.Health = 500;
                    this.MaxHealth = Health;
                    Size = new Vector3(30, 30, 0);
                    break;
                case GameConstants.TYPE_BOT_SHOOTER: // ???
                    this.Acceleration = new Vector3(2, 2, 0);
                    this.AttackRadiusSquared = 250 * 250;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 7;
                    this.Health = 30;
                    this.MaxHealth = Health;
                    Size = new Vector3(20, 20, 0);
                    break;
                case GameConstants.TYPE_BOT_TOWER: // Need to kill this to win
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 1 * 1;
                    this.ClassType = type;
                    this.Weapon = new CrossBow(this);
                    this.Damage = 0;
                    this.Health = 1500;
                    this.MaxHealth = Health;
                    Size = new Vector3(100, 100, 0);
                    break;
                case GameConstants.TYPE_BOT_TURRET: // stationary
                    this.Acceleration = new Vector3(0, 0, 0);
                    this.AttackRadiusSquared = 200 * 200;
                    this.ClassType = type;
                    this.Weapon = new Magic(this);
                    this.Damage = 16;
                    this.Health = 750;
                    this.MaxHealth = Health;
                    Size = new Vector3(60, 60, 0);
                    break;

            }
        }

        /// <summary>
        /// Called to lower the Health of the Bot
        /// </summary>
        /// <param name="val"></param>
        internal virtual void DecreaseHealth(int val)
        {
            Health -= val;
            if (Health <= 0)
            {
                Alive = false;
            }
        }

        /// <summary>
        /// Called to increase the Health of the Bot
        /// </summary>
        /// <param name="val"></param>
        internal void IncreaseHealth(int val)
        {
            Health += val;
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        /// <summary>
        /// Public way to decrease health of a Bot
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual int TakeDamage(int amount)
        {
            DecreaseHealth(amount);
            return (amount);
        }

        /// <summary>
        /// The AI for the Bot, called once per update
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void CheckInput(double deltaTime)
        {
            double targetDistance = double.MaxValue;
            Bot target = null;

            // iterate through the enemies and find the closest one
            foreach (Bot b in Manager.World.Enemies(this.Team))
            {
                double distanceFromSquared = (Position - b.Position).LengthSquared();
                // if the enemy is in range, and the closest enemy
                if (distanceFromSquared < AttackRadiusSquared && distanceFromSquared < targetDistance)
                {
                    // Set the target
                    target = b;
                    targetDistance = distanceFromSquared;
                }
            }

            // if there is an Enemy targeted, attack it / move toward it
            if (target != null)
                {
                RotateTowardPosition(target.Position);
                //if () // If the bot is facing the target, returns true
                //{
                    if (targetDistance > Weapon.Projectile.RangeSquared)
                    {
                        // Target is not close enough to attack, so move closer
                        MoveForward(deltaTime);
                    }
                    else
                    {
                        Weapon.Attack();
                    }
                //}
            }
        }

        public override void Update(double deltaTime)
        {
            // Check what to do
            CheckInput(deltaTime);

            // save previous position before moving
            Vector3 previousPosition = new Vector3(Position.x, Position.y, Position.z);

            // perform move
            base.Update(deltaTime);

            // check for new collisions
            bool collided = false;
            foreach (GameObject o in Manager.World.NotProjectiles)
            {
                // if the object is not itself, and it has collided
                if (this.ID != o.ID && this.CollidesWith(o))

                    // if the object is a projectile, dont collide with its owner
                    if (o.GetType() == typeof(GameProjectile) && ((GameProjectile)o).Owner.ID == this.ID)
                    {
                        // do nothing
                    }
                    else
                    {
                        collided = true;
                    }
            }

            // if collided, revert to the old position
            if (collided)
            {
                this.Position = previousPosition;

                if (this.Position.y >= 384)
                {
                    this.RotateTowardPosition(new Vector3(950, 700, 0));
                }
                else
                {
                    this.RotateTowardPosition(new Vector3(950, 0, 0));
                }
            }
        }
    }
}

