using System.Numerics;
using librbr.Framework.World.Room;
using Raven.Util;

namespace librbr.Framework.World.Chunk {
    public interface IChunkConfig {
        Coordinate Coordinates { get; }
        IRoomConfig[ , ] Rooms { get; }
        int Size { get; }
        Coordinate Center { get; }

        Dictionary<Direction, List<IRoomConfig>> GetOpenSides ( );
        void SetRoom (Coordinate cords, IRoomConfig room);
    }
}