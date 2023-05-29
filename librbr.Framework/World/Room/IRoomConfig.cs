using Raven.Util;

namespace librbr.Framework.World.Room {
    public interface IRoomConfig {
        IReadOnlyDictionary<Direction, bool> SideStates { get; }
        Coordinate Coordinates { get; }

        IEnumerable<Direction> GetOpenSides ( );
    }
}