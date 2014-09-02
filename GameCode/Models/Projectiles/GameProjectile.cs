using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models.Projectiles
{
    /// <summary>
    /// Represents any object that causes damage in the game, ie. Arrow, Sword, Bullet
    /// </summary>
    public abstract class GameProjectile : MovingObject
    {
        /// <summary>
        /// Location the projectile originated
        /// </summary>
        public Vector3 StartPosition;

        /// <summary>
        /// The Bot the "shot" the projectile
        /// </summary>
        internal Bot Owner;
        internal int OwnerID;
        /// <summary>
        /// The range the projectile shot fom this weapon will travel before disapearing
        /// </summary>
        public double Range { get; set; }
        /// <summary>
        /// The range the projectile shot fom this weapon will travel before disapearing
        /// </summary>
        public double RangeSquared { get; set; }
        /// <summary>
        /// The speed at which the projectile will accelerate
        /// </summary>
        //public Vector3 ProjectileSpeed { get; set; }

        /// <summary>
        /// The distance the projectile can move (stored in squared form to ease math)
        /// </summary>
        //protected double RangeSquared;

        /// <summary>
        /// The amount of damage the projectile causes
        /// </summary>
        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        
        public GameProjectile(int ownerID, GameManager manager, Vector3 size, double angle, int damage, double rangeSquared)
            : base(new Vector3(0,0,0), manager, size)
        {
            OwnerID = ownerID;
            Owner = (Bot)manager.World.Get(OwnerID);
            //StartPosition = new Vector3(Position.x, Position.y, Position.z);
            RangeSquared = rangeSquared;
            this.Damage = damage;
            this.Angle = angle;
            ClassType = GameConstants.TYPE_PROJ_ARROW;
            //Position = Owner.Position;
        }

        /// <summary>
        /// Does the movement, all sub objects should include a base.Update() to move the object
        /// </summary>
        /// <param name="deltaTime">time since last update</param>
        public override void Update(double deltaTime)
        {
            // Change velocity
            MoveForward(deltaTime);

            base.Update(deltaTime);

            // check range
            if ((Position - StartPosition).LengthSquared() > RangeSquared)
            {
                Alive = false;
            }
        }
    }
}

