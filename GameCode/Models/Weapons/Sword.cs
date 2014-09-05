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
            this.Projectile = new StabAttack(owner.ID, owner.Manager, Owner.Angle, Owner.Damage, 25)
            {
                Acceleration = new Vector3(10, 10, 0),
                Position = Owner.Position
            };
            this.RateOfFire = .762;
        }

        public override int Attack()
        {
            int projID = -1;
            if (IsReadyForNextAttack())
            {
                StabAttack p = new StabAttack(Owner.ID,
                        Owner.Manager,
                        Owner.Angle,
                        Owner.Damage,
                        Projectile.Range)
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
