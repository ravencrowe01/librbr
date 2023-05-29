
using librbr.Framework.World;
using Raven.Util;

namespace librbr.Framework {
    public class Utility {
        public static readonly Coordinate North = new Coordinate(0, 1);
        public static readonly Coordinate South = new Coordinate(0, -1);
        public static readonly Coordinate East = new Coordinate(1, 0);
        public static readonly Coordinate West = new Coordinate(-1, 0);

        public static Direction GetOppositeDirection (Direction direction) {
            var vec = GetVectorFromDirection(direction);

            return GetDirectionFromVector(vec * -1);
        }

        private static readonly Dictionary<Direction, Coordinate> DirectionToVector = new() {
            {Direction.North, North },
            {Direction.South, South },
            {Direction.East, East },
            {Direction.West, West }
        };

        public static Coordinate GetVectorFromDirection (Direction direction) => DirectionToVector[direction];

        private static readonly Dictionary<Coordinate, Direction> VectorToDirection = new() {
            {North, Direction.North },
            {South, Direction.South },
            {East, Direction.East },
            {West, Direction.West }
        };

        public static Direction GetDirectionFromVector (Coordinate vector) => VectorToDirection[vector];

        public static List<Coordinate> GetAdjacentVectors (Coordinate cords, int width, int height) {
            var list = new List<Coordinate>();

            foreach (var dir in (Direction[ ]) Enum.GetValues(typeof(Direction))) {
                var vec = GetVectorFromDirection(dir);

                var neighbor = cords + vec;

                if (neighbor.X >= 0 && neighbor.X < width && neighbor.Y >= 0 && neighbor.Y < height) {
                    list.Add(neighbor);
                }
            }

            return list;
        }
    }
}