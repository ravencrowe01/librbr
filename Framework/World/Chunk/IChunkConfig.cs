using System.Numerics;
using librbr.Framework.World.Room;

namespace librbr.Framework.World.Chunk {
    public interface IChunkConfig {
        Vector2 Coordinates { get; }
        IRoomConfig[ , ] Rooms { get; }
        int Size { get; }

        Dictionary<Direction, List<IRoomConfig>> GetOpenSides ( );
        void SetRoom (Vector2 cords, IRoomConfig room);
    }
}