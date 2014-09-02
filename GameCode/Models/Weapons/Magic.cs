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
            this.Projectile = new Arrow(owner.ID, owner.Manager, Owner.Angle, Owner.Damage, 125 * 125)
            {
                Acceleration = new Vector3(15, 15, 0)
            };
            this.RateOfFire = 1.2; // shoots 3 times per second
            //Attackype = GameConstants.TYPE_PROJ_FIRE;
        }

            public override int Attack()
        {
            int projID = -1;
            if (IsReadyForNextAttack())
            {
                FireBall p = new FireBall(Owner.ID,
                        Owner.Manager,
                        Owner.Angle,
                        Owner.Damage,
                        Projectile.RangeSquared)
                {
                    Acceleration = Projectile.Acceleration,
                    Position = Owner.Position,
                    StartPosition = Owner.Position
                };
                Owner.Manager.AddObject(p);
                projID = p.ID;
                UpdateTimeNextAvailable();
            }
            return projID;
        }
    }
}
