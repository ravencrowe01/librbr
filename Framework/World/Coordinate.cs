namespace librbr.Framework.World {
    public class Coordinate {
        public int X { get; private set; }

        public int Y { get; private set; }

        public Coordinate (int x, int y) {
            X = x;
            Y = y;
        }

        public static Coordinate operator + (Coordinate a, Coordinate b) => new Coordinate(a.X + b.X, a.Y + b.Y);

        public static Coordinate operator - (Coordinate a, Coordinate b) => new Coordinate(a.X - b.X, a.Y - b.Y);
        public static Coordinate operator - (Coordinate a) => new Coordinate(-a.X, -a.Y);

        public static Coordinate operator * (Coordinate a, Coordinate b) => new Coordinate(a.X * b.X, a.Y * b.Y);

        public static Coordinate operator / (Coordinate a, Coordinate b) => new Coordinate((int) MathF.Ceiling(a.X / b.X), (int) MathF.Ceiling(a.Y / b.Y));
    }
}