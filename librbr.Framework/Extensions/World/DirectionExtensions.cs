using librbr.Framework.World;
using Raven.Util;

namespace librbr.Framework.Extensions.World {
    public static class DirectionExtensions {
        public static Coordinate ToCoordinate (this Direction dir) => dir switch {
            Direction.North => new Coordinate(0, 1),
            Direction.South => new Coordinate(0, -1),
            Direction.East => new Coordinate(1, 0),
            Direction.West => new Coordinate(-1, 0),
            _ => new Coordinate(0, 0),
        };
    }
}