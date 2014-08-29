using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Projectiles
{
    public class Arrow : GameProjectile
    {
        public Arrow(Bot owner, GameManager manager, double angle, int damage = 10, double rangeSquared = 200) :
            base(owner,
            manager, 
            new Vector3(17, 8, 0), // Size of an arrow is always the same
            angle,
            damage, // damage is dependant on owner but we specify a default anyway
            rangeSquared)
        {
        }

        public override void Update(double deltaTime)
        {
             //perform movement
            base.Update(deltaTime);

            // if still alive, check collision
            if (Alive)
            {
                foreach (GameObject o in Manager.World.Objects)
                {
                    if (o.ID == 14)
                    {
                        //Console.WriteLine("asfd");
                    }
                    // dont check for collisions with self, owner
                    // TODO or team
                    if (/* o.Team != this.Team && */ o.ID != this.ID && o.ID != Owner.ID && this.CollidesWith(o))
                    {
                        // only apply damage if collision is with a bot
                        if (o.GetType() == typeof(Bot) || o.GetType() == typeof(Character))
                        {
                            ((Bot)o).TakeDamage(Damage);
                        }
                        // After collision, remove from play
                        Alive = false;
                    }
                }
            }
        }
    }
}
