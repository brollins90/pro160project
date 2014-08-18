using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models
{

    public class Projectile : GameObject
    {   

        public int _Team;
        public int Team
        {
            get { return _Team; }
            set { _Team = value; }
        }

        public Projectile(Vector position, float direction, int speed, int damage)
            : base(position)
        {
            this.Damage = damage;
            this.Direction = direction;
            this.Speed = speed;
        }

        public override void Update()
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
            
        }
    }
}

