using GameCode.Helpers;

namespace GameCode.Models
{
    /// <summary>
    /// A Wall is a debris that surrounds a castle
    /// </summary>
    public class CastleWalls : Debris
    {
        public CastleWalls(Vector3 position, GameManager manager, Vector3 size)
            : base(position, manager, size)
        {
            ClassType = GameConstants.TYPE_DEBRIS_WALL;
        }
    }
}
