namespace librbr.Framework.World.Area {
    public interface IAreaBuilder {
        IAreaConfig BuildArea (int rng);
        IAreaBuilder WithChunkSize (int size);
        IAreaBuilder WithSize (int size);
    }
}
