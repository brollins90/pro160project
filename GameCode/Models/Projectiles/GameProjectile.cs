using GameCode.Helpers;

namespace GameCode.Models.Projectiles
{
    /// <summary>
    /// Represents any object that causes damage in the game, ie. Arrow, Sword, Bullet
    /// </summary>
    public abstract class GameProjectile : MovingObject
    {
        /// <summary>
        /// The amount of damage the projectile causes
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// The Bot the "shot" the projectile
        /// </summary>
        internal Bot Owner { get; set; }
        internal int OwnerID { get; set; }

        /// <summary>
        /// The range the projectile shot fom this weapon will travel before disapearing
        /// </summary>
        protected double _Range;
        public double Range { get { return _Range; } set { _Range = value; } }

        /// <summary>
        /// The range the projectile shot fom this weapon will travel before disapearing (squared)
        /// </summary>
        public double RangeSquared { get { return _Range * _Range; } }

        /// <summary>
        /// Location the projectile originated
        /// </summary>
        public Vector3 StartPosition { get; set; }

        
        public GameProjectile(int ownerID, GameManager manager, Vector3 size, double angle, int damage, double range)
            : base(new Vector3(0, 0, 0), manager, size)
        {
            this.Angle = angle;
            this.Damage = damage;
            this.OwnerID = ownerID;
            this.Owner = (Bot)manager.World.Get(OwnerID);
            this.Range = range;
            this.ClassType = GameConstants.TYPE_PROJ_ARROW;

            if (Owner != null)
            {
                this.Team = Owner.Team;
            }
        }

        /// <summary>
        /// Does the movement, all sub objects should include a base.Update() to move the object
        /// </summary>
        /// <param name="deltaTime">time since last update</param>
        public override void Update(double deltaTime)
        {
            // Projectiles always move straight forward
            MoveForward(deltaTime);

            // Perform the generic move
            base.Update(deltaTime);

            // check range and remove object if moved too far
            if ((Position - StartPosition).LengthSquared() > RangeSquared)
            {
                Alive = false;
            }

            // Since projectiles die in the base class because of distance, if still alive, check collision
            if (Alive)
            {
                foreach (GameObject o in Manager.World.Objects)
                {
                    // Dont check for collisions with Team, self, owner
                    if (o.Team != this.Team && o.ID != this.ID && o.ID != Owner.ID && this.CollidesWith(o))
                    {
                        // Only apply damage if collision is with a bot (cannot hurt walls)
                        if (o.GetType() == typeof(Bot) || o.GetType() == typeof(Character))
                        {
                            Owner.Manager.DamageBot(o.ID, Damage, Owner);
                        }
                        // After collision, remove from play
                        Alive = false;
                    }
                }
            }
        }
    }
}

