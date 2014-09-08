using GameCode.Helpers;

namespace GameCode.Models.Projectiles
{
    public class StabAttack : GameProjectile
    {
        public StabAttack(int ownerID, GameManager manager, double angle, int damage = 10, double range = 25)
            : base(
            ownerID,                // The ID of the Attacker
            manager,                // A reference to the GameManager
            new Vector3(25, 12, 0), // Size of an projectile (always the same)
            angle,                  // Angle the projectile is moving
            damage,                 // The damage is dependant on owner but we specify a default anyway
            range)                  // The range the projectile will move
        {
            ClassType = GameConstants.TYPE_PROJ_STAB;
        }
    }
}
