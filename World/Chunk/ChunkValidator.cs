using librbr.Framework.World.Chunk;
using Raven.Pathfinding.AStar;
using Raven.Pathfinding.Framework.Config;

namespace librbr.World.Chunk {
    public class ChunkValidator : IChunkValidator {
        public bool ValidateChunk (IChunkConfig chunk) {
            var path = new AStarPathfinder(GeneratePFConfig(chunk));
            throw new NotImplementedException();
        }

        private IPathfinderConfig GeneratePFConfig (IChunkConfig chunk) {
            throw new NotImplementedException();
        }
    }
}