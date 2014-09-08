using GameCode.Helpers;

namespace GameCode.Models
{
    /// <summary>
    /// A Debris is an object that nothing can move through
    /// </summary>
    public class Debris : GameObject
    {
        public Debris(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            Team = GameConstants.TEAM_INT_DEBRIS;
            ClassType = GameConstants.TYPE_DEBRIS_BUSH;
        }

        /// <summary>
        /// The update for a Debris does nothing
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(double deltaTime)
        {
            return;
        }
    }
}
