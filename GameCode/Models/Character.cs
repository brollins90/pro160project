using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models
{
    public class Character : GameObject, IMovingObject, IAttackingObject
    {
        private int _Constitution;
        public int Constitution
        {
            get { return _Constitution; }
            set { _Constitution = value; }
        }

        private int _Defense;
        public int Defense
        {
            get { return _Defense; }
            set { _Defense = value; }
        }

        private int _Experience;
        public int Experience
        {
            get { return _Experience; }
            set { _Experience = value; }
        }

        private int _Level;
        public int Level
        {
            get { return _Level; }
            set { _Level = value; }
        }

        private int _Strength;
        public int Strength
        {
            get { return _Strength; }
            set { _Strength = value; }
        }

        public Character()
        {
            Constitution = 5;
            Defense = 6;
            Experience = 10;
            Level = 1;
            Strength = 3;
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
