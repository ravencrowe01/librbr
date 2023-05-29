using librbr.Framework.World.Area;
using librbr.Framework.World.Chunk;

namespace librbr.World.Area {
    public class ProtoArea : IAreaConfig {
        public IChunkConfig[,] Chunks { get; private set; }

        public int AreaSize { get; private set; }

        public ProtoArea (int areaSize) {
            AreaSize = areaSize;

            Chunks = new IChunkConfig[AreaSize, AreaSize];
        }

        public IChunkConfig GetChunk(int x, int y) => Chunks[x, y];

        public void SetChunk(IChunkConfig chunk, int x, int y) {
            throw new NotImplementedException();
        }
    }
}