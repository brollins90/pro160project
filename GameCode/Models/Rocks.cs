using GameCode.Helpers;

namespace GameCode.Models
{
    /// <summary>
    /// A Rock is a grey debris that is no different than the other debris
    /// </summary>
    public class Rocks : Debris
    {
        public Rocks(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            ClassType = GameConstants.TYPE_DEBRIS_ROCK;
        }
    }
}
