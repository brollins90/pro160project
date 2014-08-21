using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Weapons
{
    public class CrossBow : Weapon
    {
        public CrossBow(Bot owner) : base(owner)
        {
            this.ProjectileRange = 100;
            this.ProjectileSpeed = 10;
            this.RateOfFire = 2;
        }
    }
}
