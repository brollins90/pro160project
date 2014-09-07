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
            base(ownerID,           // The ID of the Attacker
            manager,                // A reference to the GameManager
            new Vector3(20, 10, 0), // Size of an projectile (always the same)
            angle,                  // Angle the projectile is moving
            damage,                 // The damage is dependant on owner but we specify a default anyway
            range)                  // The range the projectile will move
         {
             ClassType = GameConstants.TYPE_PROJ_FIRE;
        }
    }
}
