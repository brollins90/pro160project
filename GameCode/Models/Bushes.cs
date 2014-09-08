using GameCode.Helpers;

namespace GameCode.Models
{
    /// <summary>
    /// A Bush is a debris that is green and round
    /// </summary>
    public class Bushes : Debris
    {
        public Bushes(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            ClassType = GameConstants.TYPE_DEBRIS_BUSH;
        }
    }
}
