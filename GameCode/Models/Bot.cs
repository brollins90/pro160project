using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    //The main class for all Non Playable Characters
    public class Bot : GameObject, IMovingObject, IAttackingObject
    {
        public int _Speed;
        public int Speed
        {
            get { return _Speed; }
            set { _Speed = value; }
        }

        public bool _Team;
        public bool Team
        {
            get { return _Team; }
            set { _Team = value; }
        }

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

