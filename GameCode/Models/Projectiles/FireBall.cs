using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameCode.Models.Projectiles
{
     public class FireBall : GameProjectile
    {

         public FireBall(int ownerID, GameManager manager, double angle, int damage = 10, double range = 125) :
            base(ownerID,
            manager, 
            new Vector3(20, 10, 0), // Size of an arrow is always the same
            angle,
            damage, // damage is dependant on owner but we specify a default anyway
            range)
         {
             ClassType = GameConstants.TYPE_PROJ_FIRE;
        }

        public override void Update(double deltaTime)
        {
            // Nothing to override
            base.Update(deltaTime);
        }
    }
}
