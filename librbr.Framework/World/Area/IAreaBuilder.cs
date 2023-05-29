using librbr.Framework.World.Chunk;

namespace librbr.Framework.World.Area {
    public interface IAreaBuilder {
        IAreaConfig BuildArea (IChunkValidator validator, IRNGProvider rng, int seed);
        IAreaBuilder WithChunkSize (int size);
        IAreaBuilder WithSize (int size);
    }
}
