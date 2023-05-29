using librbr.Framework.World;
using Raven.Util;

namespace librbr.Framework.Extensions.World {
    public static class CoordinateExtensions {
        /// <summary>
        /// Return the direction with the greatest influence on this coordinate.
        /// </summary>
        /// <param name="cord">The coordinate to operate on.</param>
        /// <returns>A cardinal direction.</returns>
        public static Direction ToDirection (this Coordinate cord) => new Dictionary<Coordinate, Direction>(){
            { Direction.North.ToCoordinate(), Direction.North },
            { Direction.South.ToCoordinate(), Direction.South },
            { Direction.East.ToCoordinate(), Direction.East },
            { Direction.West.ToCoordinate(), Direction.West }
        }[FixCord(cord)];

        private static Coordinate FixCord (Coordinate cord) {
            // Get the absolute values of x and y.
            var absX = Math.Abs(cord.X);
            var absY = Math.Abs(cord.Y);

            // Determine if x or y has the biggest influence.
            var max = Math.Max(cord.X, cord.Y);

            // Create a new coordinate.
            // Note: This is essentially making two fractions 
            // then discarding the one that doesn't have an abs of 1.
            return new Coordinate(cord.X / max, cord.Y / max);
        }
    }
}