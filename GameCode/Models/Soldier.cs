using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameCode.Models
{
    public class Soldier : GameObject, IMovingObject, IAttackingObject
    {
        private int _Health;
        public int Health
        {
            get { return _Health; }
            set { _Health = value; }
        }

        public Soldier(Point position) : base(position)
        {
            base.Height = 10;
            base.Width = 10;
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
