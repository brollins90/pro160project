using GameCode.Helpers;
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
        private Vector3 StartPosition;
        //private Vector targetPosition;
        private double MaxDistance;

        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set { _Damage = value; }
        }
        
        public GameProjectile(Vector3 position, GameManager manager, Vector3 size, Vector3 velocity, int damage, double maxDistanceSquared)
            : base(position, manager, size)
        {
            StartPosition = new Vector3(position.x, position.y, Position.z);
            MaxDistance = maxDistanceSquared;
            this.Damage = damage;
            this.Velocity = velocity;
            //this.Speed = speed;
        }

        public override void Update(double deltaTime)
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

