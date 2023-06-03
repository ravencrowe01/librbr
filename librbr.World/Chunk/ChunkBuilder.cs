using librbr.Framework;
using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using librbr.Framework.World.Room;
using Raven.Util;

namespace librbr.World.Chunk {
    public class ChunkBuilder : IChunkBuilder {
        private int _size;

        private Dictionary<Coordinate, List<Direction>> _chunkConnections = new Dictionary<Coordinate, List<Direction>>();

        private Coordinate? _cords;

        private IRNGProvider _rng;
        private int _seed;

        public ChunkBuilder (IRNGProvider rng, int seed) {
            _rng = rng;
            _seed = seed;
        }

        private Dictionary<Direction, float> _wallWeights = new Dictionary<Direction, float> {
            { Direction.North, 1f },
            { Direction.South, 1f },
            { Direction.East, 1f },
            { Direction.West, 1f }
        };

        #region Fluent methods
        public IChunkBuilder WithDimensions (int size) {
            _size = size;
            return this;
        }

        public IChunkBuilder WithConnection (Coordinate connectedRoom, Direction dir) {
            if (!_chunkConnections.ContainsKey(connectedRoom)) {
                _chunkConnections.Add(connectedRoom, new List<Direction>());
            } else {
                var dirs = _chunkConnections[connectedRoom];

                if (!dirs.Contains(dir)) {
                    _chunkConnections[connectedRoom].Add(dir);
                }
            }

            return this;
        }

        public IChunkBuilder WithCoordinates (Coordinate cords) {
            _cords = cords;
            return this;
        }

        public IChunkBuilder WithWallWeights (float north = 1f, float south = 1f, float east = 1f, float west = 1f) {
            return this;
        }

        public IChunkConfig BuildChunk (IChunkValidator validator) {
            // TODO ChunkBuilderMissingArgsException
            if (_cords is null) {
                throw new Exception();
            }

            _seed = _seed ^ _cords.X ^ _cords.Y;

            var chunk = new ProtoChunk(_size, _cords);

            do {
                ConstructRooms(chunk);

                AddChunkConnections(chunk);

                FixRoomConnections(chunk);
            } while (!validator.ValidateChunk(chunk));

            return chunk;
        }
        #endregion

        #region Room Construction
        private void ConstructRooms (ProtoChunk chunk) {
            var center = new Coordinate((int) MathF.Floor(_size / 2f), (int) MathF.Floor(_size / 2f));

            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    chunk.Rooms[x, y] = ConstructRoom(x, y, center);
                }
            }
        }

        private IRoomConfig ConstructRoom (int x, int y, Coordinate center) {
            var room = new ProtoRoom(new Coordinate(x, y));
            _rng.SetSeed(_seed ^ x ^ y);

            AddOpenSides(room, GetOpenableSides(x, y), room.Coordinates == center ? 1 : 2);

            return room;
        }

        private IList<Direction> GetOpenableSides (int x, int y) {
            var sides = new List<Direction>();

            // TODO RoomOutOfBoundsException
            if (x < 0 || x > _size || y < 0 || y > _size) {
                throw new IndexOutOfRangeException($"[{GetType().Name}.{nameof(GetOpenableSides)}]: Tried to access out of bounds room {{{x}, {y}}}");
            }

            if (x == 0) {
                sides.Add(Direction.East);
            } else if (x == _size - 1) {
                sides.Add(Direction.West);
            } else {
                sides.AddRange(new List<Direction> { Direction.East, Direction.West });
            }

            if (y == 0) {
                sides.Add(Direction.North);
            } else if (y == _size - 1) {
                sides.Add(Direction.South);
            } else {
                sides.AddRange(new List<Direction> { Direction.North, Direction.South });
            }

            return sides;
        }

        private void AddOpenSides (ProtoRoom room, IList<Direction> directions, int min) {
            var tempDir = new List<Direction>(directions);

            var amt = _rng.Range(min, tempDir.Count);

            var attempts = 0;

            for (; attempts < 10 && amt > 0; attempts++) {
                var chosenDir = tempDir[_rng.Range(0, tempDir.Count)];
                var roll = _rng.Range(1f);

                if (roll <= _wallWeights[chosenDir]) {
                    room.OpenSide(chosenDir);

                    tempDir.Remove(chosenDir);

                    amt--;
                }
            }

            if (attempts >= 10) {
                for (int i = 1; i <= min; i++) {
                    room.OpenSide(directions[_rng.Range(0, directions.Count - 1)]);
                }
            }
        }
        #endregion

        #region Chunk Connecting
        private void AddChunkConnections (ProtoChunk chunk) {
            if (_chunkConnections.Count == 0) {
                CreateRandomConnections(2, _size * 2);
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
                var dir = (Direction) _rng.Range(0, 3);

                var room = _rng.Range(0, _size - 1);

                Coordinate roomCords = GetConnectionRoomCoordinates(dir, room);

                if (_chunkConnections.ContainsKey(roomCords)) {
                    if (!_chunkConnections[roomCords].Contains(dir)) {
                        _chunkConnections[roomCords].Add(dir);

                        roll--;
                    }
                    /* TODO ChunkBuilder Infinite Connection Loop
                     * Okay, so. There's a statistically significant chance that the same room (with the same side
                     * but most rooms only have one 'connection' side) get's chosen repeatedly, which theoretically
                     * could cause the thread to lock up. However, unless a chunk is *far* larger than it should be,
                     * I'm willing to take the chance that this is something that needs to be worried about.
                     */
                } else {
                    _chunkConnections.Add(roomCords, new List<Direction>() { dir });

                    roll--;
                }
            }
        }

        private Coordinate GetConnectionRoomCoordinates (Direction dir, int room) {
            if (dir == Direction.North) {
                return new Coordinate(room, _size - 1);
            } else if (dir == Direction.South) {
                return new Coordinate(room, 0);
            } else if (dir == Direction.East) {
                return new Coordinate(_size - 1, room);
            } else if (dir == Direction.West) {
                return new Coordinate(0, room);
            }

            return new Coordinate(-1, -1);
        }
        #endregion

        #region Room Connection Fixing
        private void FixRoomConnections (ProtoChunk chunk) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    _rng.SetSeed(_seed ^ x ^ y);

                    var room = new Coordinate(x, y);

                    var adjacent = Utility.GetAdjacentCoordinates(room, _size, _size);

                    foreach (var neighbor in adjacent) {
                        var dir = Utility.GetDirectionFromVector(neighbor - room);

                        var neighborDir = Utility.GetDirectionFromVector(room - neighbor);

                        if (chunk.Rooms[x, y].SideStates[dir] != chunk.Rooms[(int) neighbor.X, (int) neighbor.Y].SideStates[neighborDir]) {
                            var roll = _rng.Range(0, 1) == 1;

                            chunk.SetRoomSide(room, dir, roll);

                            chunk.SetRoomSide(neighbor, neighborDir, roll);
                        }
                    }
                }
            }
        }
        #endregion
    }
}