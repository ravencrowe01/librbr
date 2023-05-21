
using System.Numerics;
using librbr.Framework.World;

namespace librbr.Framework {
    public class Utility {
        public static readonly Vector2 North = new Vector2(0, 1);
        public static readonly Vector2 South = new Vector2(0, -1);
        public static readonly Vector2 East = new Vector2(1, 0);
        public static readonly Vector2 West = new Vector2(-1, 0);


        public static Direction GetOppositeDirection (Direction direction) {
            var vec = GetVectorFromDirection(direction);

            return GetDirectionFromVector(vec * -1);
        }

        private static readonly Dictionary<Direction, Vector2> DirectionToVector = new() {
            {Direction.North, North },
            {Direction.South, South },
            {Direction.East, East },
            {Direction.West, West }
        };

        public static Vector2 GetVectorFromDirection (Direction direction) => DirectionToVector[direction];

        private static readonly Dictionary<Vector2, Direction> VectorToDirection = new() {
            {North, Direction.North },
            {South, Direction.South },
            {East, Direction.East },
            {West, Direction.West }
        };

        public static Direction GetDirectionFromVector (Vector2 vector) => VectorToDirection[vector];

        public static List<Vector2> GetAdjacentVectors (Vector2 cords, int width, int height) {
            var list = new List<Vector2>();

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