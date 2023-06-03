using librbr.Framework.World.Chunk;

namespace librbr.Framework.World.Area {
    public interface IAreaBuilder {
        IAreaConfig BuildArea (IChunkValidator validator);
        IAreaBuilder WithChunkSize (int size);
        IAreaBuilder WithDimensions (int width, int height);
    }
}
