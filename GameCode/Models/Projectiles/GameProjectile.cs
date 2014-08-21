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

    public abstract class GameProjectile : MovingObject
    {
        private Vector3 StartPosition;
        //private Vector targetPosition;
        private double RangeSquared;

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        public GameProjectile(Vector3 position, GameManager manager, Vector3 size, double angle, int damage, double rangeSquared)
            : base(position, manager, size)
        {
            StartPosition = new Vector3(position.x, position.y, Position.z);
            RangeSquared = rangeSquared;
            this.Damage = damage;
            this.Angle = angle;
        }

        public override void Update(double deltaTime)
        {
            Velocity = Velocity - (deltaTime * Acceleration * -1 * Heading);
            base.Update(deltaTime);
            if ((Position - StartPosition).LengthSquared() > RangeSquared)
            {
                Alive = false;
            }
        }
    }
}

