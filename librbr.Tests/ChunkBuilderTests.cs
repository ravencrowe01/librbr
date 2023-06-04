namespace librbr.Tests;

public class ChunkBuilderTests {
    private int _seedA = 42069;

    [Test]
    public void ChunkBuilder_SeedA_Valid() {
        var builder = new ChunkBuilder(new RNGProvider(_seedA), _seedA);

        builder.WithCoordinates(new Coordinate(0, 0)).WithDimensions(3);

        var chunk = builder.BuildChunk(new ChunkValidator());

        // System.Console.WriteLine((chunk as ProtoChunk).ToString());

        Assert.That(chunk, Is.Not.Null);
    }

    private class RNGProvider : IRNGProvider {
        private Random rng;

        public RNGProvider (int seed) {
            rng = new Random(seed);
        }

        public int Range (int max) => rng.Next(max + 1);

        public int Range (int min, int max) => rng.Next(min, max + 1);

        public float Range (float max) => (float) (rng.NextDouble() * max);

        public float Range (float min, float max) {
            float roll = (float) (rng.NextDouble() * max);

            while(roll < min) {
                roll = (float) (rng.NextDouble() * max);
            }

            return roll;
        }

        public void SetSeed (int seed) {
            rng = new Random(seed);
        }
    }
}