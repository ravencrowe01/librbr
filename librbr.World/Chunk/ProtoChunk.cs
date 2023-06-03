using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using librbr.Framework.World.Room;
using Raven.Util;

namespace librbr.World {
    public class ProtoChunk : IChunkConfig {
        public Coordinate Coordinates { get; }

        public IRoomConfig[ , ] Rooms => _rooms;
        private ProtoRoom[ , ] _rooms;

        public int Size { get; }
        public Coordinate Center { get; }

        public ProtoChunk (int size, Coordinate cords) {
            Size = size;
            Coordinates = cords;
            _rooms = new ProtoRoom[size, size];
            Center = new Coordinate(Size / 2, Size / 2);
        }

        public Dictionary<Direction, List<IRoomConfig>> GetOpenSides ( ) {
            var open = new Dictionary<Direction, List<IRoomConfig>>();

            for (int i = 0; i < Size; i++) {
                foreach (Direction dir in Enum.GetValues(typeof(Direction))) {
                    open.Add(dir, new List<IRoomConfig>());

                    if (_rooms[i, 0].SideStates[dir]) {
                        open[dir].Add(_rooms[i, 0]);
                    }

                    if (_rooms[i, Size - 1].SideStates[dir]) {
                        open[dir].Add(_rooms[i, Size - 1]);
                    }

                    if (_rooms[0, i].SideStates[dir]) {
                        open[dir].Add(_rooms[0, i]);
                    }

                    if (_rooms[Size - 1, i].SideStates[dir]) {
                        open[dir].Add(_rooms[Size - 1, i]);
                    }
                }
            }

            return open;
        }

        public void SetRoom (Coordinate cord, IRoomConfig room) => _rooms[(int) cord.X, (int) cord.Y] = (ProtoRoom) room;

        public void SetRoomSide (Coordinate room, Direction dir, bool state) => _rooms[(int) room.X, (int) room.Y].SetSideState(dir, state);

        public void OpenRoomSide (Coordinate room, Direction dir) => _rooms[(int) room.X, (int) room.Y].OpenSide(dir);

        public void CloseRoomSide (Coordinate room, Direction dir) => _rooms[(int) room.X, (int) room.Y].CloseSide(dir);
    }
}