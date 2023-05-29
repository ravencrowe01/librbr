using Raven.Pathfinding.Framework.Config;

namespace librbr.Framework.World.Chunk {
    public interface IChunkValidator {
        bool ValidateChunk (IChunkConfig chunk);
    }
}