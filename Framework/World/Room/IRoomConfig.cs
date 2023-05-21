using System.Numerics;

namespace librbr.Framework.World.Room {
    public interface IRoomConfig {
        IReadOnlyDictionary<Direction, bool> SideStates { get; }
        Vector2 Coordinates { get; }

        IEnumerable<Direction> GetOpenSides ( );
    }
}