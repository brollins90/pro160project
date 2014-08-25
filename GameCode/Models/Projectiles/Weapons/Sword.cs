using GameCode.Helpers;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Weapons
{
    public class Sword : Weapon
    {
        public Sword(Bot owner) : base(owner)
        {
            this.ProjectileRange = 25;
            this.ProjectileSpeed = new Vector3(10, 10, 0);
            this.RateOfFire = 1.0;
        }

        public override void Attack()
        {
            if (IsReadyForNextAttack())
            {
                Owner.Manager.AddProjectile(
                    new StabAttack(Owner,
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
