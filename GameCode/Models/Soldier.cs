using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameCode.Models
{
    public class Soldier : GameObject, IMovingObject, IAttackingObject
    {
        public Soldier(Point position) : base(position)
        {

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
