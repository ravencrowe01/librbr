using librbr.Framework.World.Chunk;

namespace librbr.Framework.World.Area {
    public interface IAreaConfig {
        IChunkConfig[ , ] Chunks { get; }

        void SetChunk (IChunkConfig chunk, int x, int y);

        IChunkConfig GetChunk (int x, int y);
    }
}