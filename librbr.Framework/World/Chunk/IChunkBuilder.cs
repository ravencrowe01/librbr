using Raven.Util;

namespace librbr.Framework.World.Chunk {
    public interface IChunkBuilder {
        IChunkBuilder WithConnection (Coordinate connectedRoom, Direction dir);
        IChunkBuilder WithCoordinates (Coordinate cords);
        IChunkBuilder WithDiminsions (int size);
        IChunkConfig BuildChunk (IChunkValidator validator, IRNGProvider rng, int seed);
    }
}