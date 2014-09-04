using GameCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCode.Models.Projectiles
{
    public class StabAttack : GameProjectile
    {
        public StabAttack(int ownerID, GameManager manager, double angle, int damage = 10, double range = 25) :
            base(ownerID,
            manager,
            new Vector3(25, 12, 0), // Size of an arrow is always the same
            angle,
            damage, // damage is dependant on owner but we specify a default anyway
            range)
        {
            ClassType = GameConstants.TYPE_PROJ_STAB;
        }

        public override void Update(double deltaTime)
        {
            // Nothing to override
            base.Update(deltaTime);
        }
    }
}
