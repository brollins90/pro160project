using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Projectiles
{
    public class Arrow : GameProjectile
    {
        public Arrow(Bot owner, GameManager manager, double angle, int damage = 10, double rangeSquared = 100) :
            base(owner, manager, new Vector3(30, 30, 0), angle, damage, rangeSquared)
        {

            }
    }
}
