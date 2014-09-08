using GameCode.Helpers;
using GameCode.Models.Projectiles;

namespace GameCode.Models.Weapons
{
    public class CrossBow : Weapon
    {
        public CrossBow(Bot owner)
            : base(owner)
        {
            this.Projectile = new Arrow(owner.ID, owner.Manager, Owner.Angle, Owner.Damage, 100)
            {
                Acceleration = new Vector3(12, 12, 0),
                Position = Owner.Position
            };
            this.RateOfFire = 1.1;
        }

        public override int Attack()
        {
            int projID = -1;
            if (IsReadyForNextAttack())
            {
                Arrow p = new Arrow(Owner.ID,
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
