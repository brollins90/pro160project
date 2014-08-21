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
            this.ProjectileRange = 400;
            this.ProjectileSpeed = 5;
            this.RateOfFire = .3; // shoots 3 times per second
        }
        public override void Attack()
        {
            if (IsReadyForNextAttack()) { 
                Owner.Manager.AddProjectile(
                    new Arrow(Owner,
                        Owner.Manager,
                        Owner.Angle,
                        Owner.Damage,
                        ProjectileRange * ProjectileRange)
                        {
                            Acceleration = new Vector3(ProjectileSpeed, ProjectileSpeed, 0)
                        });
                UpdateTimeNextAvailable();
            }
        }
    }
}
