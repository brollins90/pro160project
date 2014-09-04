using GameCode.Helpers;
using GameCode.Models.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models.Weapons
{
    /// <summary>
    /// The implementation of the object that handles an attack
    /// </summary>
    public abstract class Weapon
    {
        /// <summary>
        /// Who is holding the weapon
        /// </summary>
        public Bot Owner { get; set; }

        /// <summary>
        /// The projectile to clone when firing
        /// </summary>
        public GameProjectile Projectile { get; set; }

        /// <summary>
        /// The rate of fire in number of seconds
        /// </summary>
        public double RateOfFire { get; set; }
        
        /// <summary>
        /// The counter that holds the next time the weapon will become available to fire
        /// </summary>
        public double TimeNextAvailable { get; set; }

        public Weapon(Bot owner, double rateOfFire = 1.0, double distance = 100.0, double speed = 20)
        {
            this.Owner = owner;
            //this.Projectile = new Arrow(Owner.ID, Owner.Manager, 0)
            //{
            //    Range = distance,
            //    Acceleration = new Vector3(speed, speed,0)
            //};
            this.RateOfFire = rateOfFire;
            this.TimeNextAvailable = Environment.TickCount;
        }

        /// <summary>
        /// Has enough time elapsed that the weapon can fire again?
        /// </summary>
        /// <returns></returns>
        public bool IsReadyForNextAttack()
        {
            if (Environment.TickCount > TimeNextAvailable)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the counter for the next attack time
        /// </summary>
        public void UpdateTimeNextAvailable()
        {
            TimeNextAvailable = Environment.TickCount + (1000.0 * RateOfFire);
        }

        /// <summary>
        /// Performs the attack with the weapon
        /// </summary>
        public abstract int Attack();
    }
}
