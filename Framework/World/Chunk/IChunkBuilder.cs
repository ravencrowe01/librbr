using System.Numerics;

namespace librbr.Framework.World.Chunk {
    public interface IChunkBuilder {
        IChunkConfig BuildChunk (int seed, IChunkValidator validator);
        IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir);
        IChunkBuilder WithCoordinates (Vector2 cords);
        IChunkBuilder WithDiminsions (int size);
    }
}