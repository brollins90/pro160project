using GameCode.Helpers;
using GameCode.Models.Projectiles;
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
            this.ProjectileSpeed = new Vector3(12,12,0);
            this.RateOfFire = 1.1; // shoots 3 times per second
        }
        public override void Attack()
        {
            if (IsReadyForNextAttack()) { 
                Owner.Manager.Add(
                    new Arrow(Owner,
                        Owner.Manager,
                        Owner.Angle,
                        Owner.Damage,
                        ProjectileRange * ProjectileRange)
                        {
                            Acceleration = ProjectileSpeed
                        });
                UpdateTimeNextAvailable();
            }
        }
    }
}
