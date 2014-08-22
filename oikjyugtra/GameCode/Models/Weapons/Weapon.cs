using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models.Weapons
{
    public abstract class Weapon
    {
        public Bot Owner { get; set; }
        public double RateOfFire { get; set; }
        public double TimeNextAvailable { get; set; }
        public double ProjectileRange { get; set; }
        public double ProjectileSpeed { get; set; }

        public Weapon(Bot owner, double rateOfFire = 1.0, double distance = 100.0, double speed = 7)
        {
            Owner = owner;
            RateOfFire = rateOfFire;
            TimeNextAvailable = Environment.TickCount;
            ProjectileRange = distance;
            ProjectileSpeed = speed;
        }

        public bool IsReadyForNextAttack()
        {
            if (Environment.TickCount > TimeNextAvailable)
            {
                return true;
            }
            return false;
        }

        public void UpdateTimeNextAvailable()
        {
            TimeNextAvailable = Environment.TickCount + (1000.0 * RateOfFire);
        }

        public void AimAt(Vector target)
        {
            //Owner.RotateTowardPosition(target);
        }

        public void ShootAt(Vector target)
        {
            //Owner.Manager.AddProjectile(new GameProjectile(Owner.Position, Owner.Manager, Owner.Direction, Speed, Owner.Damage, Distance));
        }

        public abstract void Attack();
    }
}
