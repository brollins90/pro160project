using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{

    public class GameProjectile : GameObject
    {
        private Vector StartPosition;
        private double MaxDistance;
        
        public GameProjectile(Vector position, GameManager manager, float direction, int speed, int damage, double maxDistanceSquared)
            : base(position, manager)
        {
            StartPosition = new Vector(position.X, position.Y);
            MaxDistance = maxDistanceSquared;
            this.Damage = damage;
            this.Direction = direction;
            this.Speed = speed;
        }

        public override void Update(int deltaTime)
        {
            if (Direction == 0)
            {
                Position = new Vector(Position.X + Speed, Position.Y);
            }
            else if (Direction == 90)
            {
                Position = new Vector(Position.X, Position.Y - Speed);
            }
            else if (Direction == 180)
            {
                Position = new Vector(Position.X - Speed, Position.Y);
            }
            else if (Direction == 270)
            {
                Position = new Vector(Position.X, Position.Y + Speed);
            }
            double distance = (Position - StartPosition).Length;
            if (distance > MaxDistance)
            {
                this.Alive = false;
            }
        }
    }
}

