namespace librbr.Framework.World.Config {
    public interface IGameWorldConfig {
        string Name { get; }

        int StartingAreaSize { get; }

        int ChunkSize { get; }
        int ChunkHeight { get; }

        int RoomSize { get; }

        int Seed { get; }
    }
}
