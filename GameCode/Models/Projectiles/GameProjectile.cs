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
        private Bot Owner;
        //private Vector targetPosition;
        private double RangeSquared;

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }

        public GameProjectile(Bot owner, GameManager manager, Vector3 size, double angle, int damage, double rangeSquared)
            : base(owner.Position, manager, size)
        {
            Owner = owner;
            StartPosition = new Vector3(Position.x, Position.y, Position.z);
            RangeSquared = rangeSquared;
            this.Damage = damage;
            this.Angle = angle;
        }

        public override void Update(double deltaTime)
        {
            Velocity = Velocity - (deltaTime * Acceleration * -1 * Heading);
            base.Update(deltaTime);
            //Console.WriteLine("Arrow");
            //Console.WriteLine("owner.id: " + Owner.ID);
            foreach (GameObject o in Manager.World.Objects)
            {
                //Console.WriteLine("o.id: " + o.ID);
                if (o.ID != this.ID && o.ID != Owner.ID && this.CollidesWith(o))
                {
                    //Console.WriteLine("collides with: " + o.ID);
                    if (o.GetType() == typeof(Bot))
                    {
                        ((Bot)o).TakeDamage(Damage);
                    }
                    Alive = false;
                    
                }
            }

            if ((Position - StartPosition).LengthSquared() > RangeSquared)
            {
                Alive = false;
            }
        }
    }
}

