using System.Numerics;
using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using librbr.Framework.World.Room;

namespace librbr.World {
    public class ProtoChunk : IChunkConfig {
        public Vector2 Coordinates { get; set; }

        public IRoomConfig[,] Rooms => _rooms;
        private ProtoRoom[,] _rooms;

        public int Size { get; }

        public ProtoChunk (int size) {
            Size = size;
            _rooms = new ProtoRoom[size, size];
        }

        public Dictionary<Direction, List<IRoomConfig>> GetOpenSides() {
            var open = new Dictionary<Direction, List<IRoomConfig>>();

            for(int i = 0; i < Size; i++) {
                foreach (Direction dir in Enum.GetValues(typeof(Direction))){
                    open.Add(dir, new List<IRoomConfig>());

                    if(_rooms[i, 0].SideStates[dir]) {
                        open[dir].Add(_rooms[i, 0]);
                    }

                    if(_rooms[i, Size - 1].SideStates[dir]) {
                        open[dir].Add(_rooms[i, Size - 1]);
                    }

                    if(_rooms[0, i].SideStates[dir]) {
                        open[dir].Add(_rooms[0, i]);
                    }

                    if(_rooms[Size - 1, i].SideStates[dir]) {
                        open[dir].Add(_rooms[Size - 1, i]);
                    }
                }
            }

            return open;
        }

        public void SetRoom(Vector2 cord, IRoomConfig room) => _rooms[(int) cord.X, (int) cord.Y] = (ProtoRoom) room;

        public void SetRoomSide (Vector2 room, Direction dir, bool state) => _rooms[(int) room.X, (int) room.Y].SetSideState(dir, state);

        public void OpenRoomSide (Vector2 room, Direction dir) => _rooms[(int) room.X, (int) room.Y].OpenSide(dir);

        public void CloseRoomSide (Vector2 room, Direction dir) => _rooms[(int) room.X, (int) room.Y].CloseSide(dir);
    }
}