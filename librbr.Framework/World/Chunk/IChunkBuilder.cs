using Raven.Util;

namespace librbr.Framework.World.Chunk {
    public interface IChunkBuilder {
        IChunkBuilder WithConnection (Coordinate connectedRoom, Direction dir);
        IChunkBuilder WithCoordinates (Coordinate cords);
        IChunkBuilder WithDimensions (int size);
        IChunkBuilder WithWallWeights (float north = 1f, float south = 1f, float east = 1f, float west = 1f);
        IChunkConfig? BuildChunk (IChunkValidator validator);
    }
}