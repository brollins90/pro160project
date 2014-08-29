using GameCode.Helpers;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameCode.Models.Weapons
{
    public class Magic : Weapon
    {
        public Magic(Bot owner) : base(owner)
        {
            this.ProjectileRange = 125;
            this.ProjectileSpeed = new Vector3(15,15,0);
            this.RateOfFire = 1.2; // shoots 3 times per second
        }

            public override void Attack()
        {
            if (IsReadyForNextAttack()) { 
                Owner.Manager.Add(
                    new FireBall(Owner,
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
