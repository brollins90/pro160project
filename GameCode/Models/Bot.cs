using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public class Bot : GameObject, IMovingObject, IAttackingObject
    {
        private int _Health;
        public int Health
        {
            get { return _Health; }
            set { _Health = value; }
        }

        public Bot(Point position) : base(position)
        {
            base.Height = 20;
            base.Width = 30;
        }

        public void Attack(Point destination)
        {
            throw new NotImplementedException();
        }

        public void Move(Point destination)
        {
            throw new NotImplementedException();
        }
    }
}

