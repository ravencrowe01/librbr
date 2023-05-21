using System.Numerics;
using librbr.Framework;
using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using librbr.Framework.World.Room;

namespace librbr.World.Chunk {
    public class ChunkBuilder : IChunkBuilder {
        private int _size;

        private Dictionary<Vector2, List<Direction>> _chunkConnections = new Dictionary<Vector2, List<Direction>>();

        private Vector2 _cords;

        private IRNGProvider _rng;

        #region Fluent methods
        public IChunkBuilder WithDiminsions (int size) {
            _size = size;
            return this;
        }

        public IChunkBuilder WithConnection (Vector2 connectedRoom, Direction dir) {
            if (!_chunkConnections.ContainsKey (connectedRoom)) {
                _chunkConnections.Add (connectedRoom, new List<Direction>());
            }
            else {
                var dirs = _chunkConnections[connectedRoom];

                if(!dirs.Contains(dir)) {
                    _chunkConnections[connectedRoom].Add(dir);
                }
            }

            return this;
        }

        public IChunkBuilder WithCoordinates (Vector2 cords) {
            _cords = cords;
            return this;
        }

        public IChunkConfig BuildChunk (int seed, IChunkValidator validator) {
            var chunk = new ProtoChunk (_size);

            do {
                ConstructRooms (chunk, seed);

                AddChunkConnections (chunk);

                FixRoomConnections (chunk, seed);
            } while (!validator.ValidateChunk(chunk));

            return chunk;
        }
        #endregion

        #region Room Construction
        private void ConstructRooms (ProtoChunk chunk, int seed) {
            var center = new Vector2(MathF.Floor(_size / 2f), MathF.Floor(_size / 2f));

            for(int x = 0; x < _size; x++) {
                for(int z = 0; z < _size; z++) {
                    chunk.Rooms[x, z] = ConstructRoom(x, z, seed, center);
                }
            }
        }

        private IRoomConfig ConstructRoom (int x, int z, int seed, Vector2 center) {
            var room = new ProtoRoom();
            room.Coordinates = new Vector2(x, z);

            AddOpenSides(room, GetOpenableSides(x, z), seed, room.Coordinates == center ? 1 : 2);

            return room;
        }

        private IList<Direction> GetOpenableSides (int x, int y) {
            var sides = new List<Direction> ();

            // TODO RoomOutOfBoundsException
            if (x < 0 || x > _size || y < 0 || y > _size) {
                throw new IndexOutOfRangeException ($"[{GetType ().Name}.{nameof (GetOpenableSides)}]: Tried to access out of bounds room {{{x}, {y}}}");
            }

            if (x == 0) {
                sides.Add (Direction.East);
            }
            else if (x == _size - 1) {
                sides.Add (Direction.West);
            }
            else {
                sides.AddRange (new List<Direction> { Direction.East, Direction.West });
            }

            if (y == 0) {
                sides.Add (Direction.North);
            }
            else if (y == _size - 1) {
                sides.Add (Direction.South);
            }
            else {
                sides.AddRange (new List<Direction> { Direction.North, Direction.South });
            }

            return sides;
        }

        private void AddOpenSides (ProtoRoom room, IList<Direction> directions, int seed, int min) {
            _rng.SetSeed (seed ^ (int) room.Coordinates.X ^ (int) room.Coordinates.Y);
            var amt = _rng.Range (min, directions.Count);

            while (amt > 0) {
                var chosenDir = directions[_rng.Range (0, directions.Count)];

                room.OpenSide(chosenDir);

                directions.Remove (chosenDir);

                amt--;
            }
        }
        #endregion

        #region Chunk Connecting
        private void AddChunkConnections (ProtoChunk chunk) {
            if (_chunkConnections.Count == 0) {
                CreateRandomConnections (2 - _chunkConnections.Count, _size * 2);
            }

            foreach (var cord in _chunkConnections.Keys) {
                foreach (var side in _chunkConnections[cord]) {
                    chunk.OpenRoomSide(cord, side);
                }
            }
        }

        private void CreateRandomConnections (int min, int max) {
            var roll = _rng.Range(min, max);

            while (roll > 0) {
                var dir = (Direction) _rng.Range (0, 4);

                var room = _rng.Range (0, _size);

                Vector2 roomCords = GetConnectionRoomCoordinates (dir, room);

                if (_chunkConnections.ContainsKey (roomCords)) {
                    if(!_chunkConnections[roomCords].Contains(dir)) {
                        _chunkConnections[roomCords].Add(dir);

                        roll--;
                    }
                } else {
                    _chunkConnections.Add(roomCords, new List<Direction>() { dir });

                    roll--;
                }
            }
        }

        private Vector2 GetConnectionRoomCoordinates (Direction dir, int room) {
            if (dir == Direction.North) {
                return new Vector2 (room, _size - 1);
            }

            else if (dir == Direction.South) {
                return new Vector2 (room, 0);
            }

            else if (dir == Direction.East) {
                return new Vector2 (_size - 1, room);
            }

            else if (dir == Direction.West) {
                return new Vector2 (0, room);
            }

            return new Vector2(-1, -1);
        }
        #endregion

        #region Room Connection Fixing
        private void FixRoomConnections (ProtoChunk chunk, int seed) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    _rng.SetSeed (seed ^ x ^ y);

                    var room = new Vector2 (x, y);

                    var adjacent = Utility.GetAdjacentVectors (room, _size, _size);

                    foreach (var neighbor in adjacent) {
                        var dir = Utility.GetDirectionFromVector (neighbor - room);

                        var neighborDir = Utility.GetDirectionFromVector (room - neighbor);

                        if (chunk.Rooms[x, y].SideStates[dir] != chunk.Rooms[(int) neighbor.X, (int) neighbor.Y].SideStates[neighborDir]) {
                            var roll = _rng.Range (0, 2) == 1;

                            chunk.SetRoomSide(room, dir, roll);

                            chunk.SetRoomSide (neighbor, neighborDir, roll);
                        }
                    }
                }
            }
        }
        #endregion
    }
}