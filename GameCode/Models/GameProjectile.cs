using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{

    public class GameProjectile : MovingObject
    {
        private Vector StartPosition;
        //private Vector targetPosition;
        private double MaxDistance;

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }
        
        public GameProjectile(Vector position, GameManager manager, Vector size, Vector velocity, int damage, double maxDistanceSquared)
            : base(position, manager, size)
        {
            StartPosition = new Vector(position.X, position.Y);
            MaxDistance = maxDistanceSquared;
            this.Damage = damage;
            this.Velocity = velocity;
            //this.Speed = speed;
        }

        public override void Update(float deltaTime)
        {
            Position = Position + Velocity * deltaTime;

            //if (Direction == 0)
            //{
            //    Position = new Vector(Position.X + Speed, Position.Y);
            //}
            //else if (Direction == 90)
            //{
            //    Position = new Vector(Position.X, Position.Y - Speed);
            //}
            //else if (Direction == 180)
            //{
            //    Position = new Vector(Position.X - Speed, Position.Y);
            //}
            //else if (Direction == 270)
            //{
            //    Position = new Vector(Position.X, Position.Y + Speed);
            //}
            //double distance = (Position - StartPosition).Length;
            //if (distance > MaxDistance)
            //{
            //    this.Alive = false;
            //}
        }
    }
}

