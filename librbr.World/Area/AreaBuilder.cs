using librbr.Framework;
using librbr.Framework.World;
using librbr.Framework.World.Area;
using librbr.Framework.World.Chunk;
using librbr.World.Chunk;
using Raven.Util;

namespace librbr.World.Area {
    public class AreaBuilder : IAreaBuilder {
        private int _width;
        private int _height;

        private int _chunkSize;

        private IRNGProvider _rng;
        private int _seed;

        public AreaBuilder (IRNGProvider rng, int seed) {
            _rng = rng;
            _seed = seed;
        }

        public IAreaBuilder WithDimensions (int width, int height) {
            _width = width;
            _height = height;
            return this;
        }

        public IAreaBuilder WithChunkSize (int size) {
            _chunkSize = size;
            return this;
        }

        public IAreaConfig BuildArea (IChunkValidator validator) {
            throw new NotImplementedException();
        }

        private void AddSeedChunks (IChunkValidator validator, ProtoArea area) {
            var a = _width * _height;

            for (int i = 0; i < a / 10 + 1; i++) {
                int x, y;

                do {
                    x = _rng.Range(0, _width);
                    y = _rng.Range(0, _height);
                } while (area.GetChunk(x, y) is not null);

                var chunk = BuildChunk(validator, area, new Coordinate(x, y));

                area.SetChunk(chunk, x, y);
            }
        }

        private void AddChunks (IChunkValidator validator, ProtoArea area) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (area.GetChunk(x, y) is null) {
                        var chunk = BuildChunk(validator, area, new Coordinate(x, y));
                        area.SetChunk(chunk, x, y);
                    }
                }
            }
        }

        private IChunkConfig BuildChunk (IChunkValidator validator, ProtoArea area, Coordinate cords) {
            var builder = new ChunkBuilder(_rng, _seed).WithDimensions(_chunkSize).WithCoordinates(cords);

            foreach (var dir in (Direction[ ]) Enum.GetValues(typeof(Direction))) {
                var nCords = cords + Utility.GetVectorFromDirection(dir);

                if (nCords.X >= 0 && nCords.X < _width && nCords.Y >= 0 && nCords.Y < _height) {
                    var neighbor = area.GetChunk((int) nCords.X, (int) nCords.Y);

                    if (neighbor is not null) {
                        AddConnections(builder, dir, neighbor as ProtoChunk); // There's a null check right there, but go off I guess.
                    }
                }
            }

            return builder.BuildChunk(validator);
        }

        private void AddConnections (IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            var opDir = Utility.GetOppositeDirection(dir);

            var open = neighbor.GetOpenSides()[opDir];

            foreach (var op in open) {
                var target = GetConnectionTarget(op.Coordinates, dir);

                builder.WithConnection(target, dir);
            }
        }

        private Coordinate GetConnectionTarget (Coordinate origin, Direction dir) {
            if (dir == Direction.North) {
                return new Coordinate(origin.X, _chunkSize - 1);
            }

            if (dir == Direction.South) {
                return new Coordinate(origin.X, 0);
            }

            if (dir == Direction.East) {
                return new Coordinate(_chunkSize - 1, origin.Y);
            }

            if (dir == Direction.West) {
                return new Coordinate(0, origin.Y);
            }

            return new Coordinate(-1, -1);
        }

        private void FixChunkConnections (ProtoArea area) {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var chunk = area.GetChunk(x, y);

                    foreach (var dir in (Direction[ ]) Enum.GetValues(typeof(Direction))) {
                        var adjVec = Utility.GetVectorFromDirection(dir) + new Coordinate(x, y);

                        if (adjVec.X >= 0 && adjVec.X < _width && adjVec.Y >= 0 && adjVec.Y < _height) {
                            var adjChunk = area.GetChunk((int) adjVec.X, (int) adjVec.Y);

                            for (int i = 0; i < _chunkSize; i++) {
                                Coordinate roomVec;
                                Coordinate adjRoomVec;

                                if (dir == Direction.North) {
                                    roomVec = new Coordinate(i, _chunkSize - 1);
                                    adjRoomVec = new Coordinate(i, 0);
                                } else if (dir == Direction.South) {
                                    roomVec = new Coordinate(i, 0);
                                    adjRoomVec = new Coordinate(i, _chunkSize - 1);
                                } else if (dir == Direction.East) {
                                    roomVec = new Coordinate(0, i);
                                    adjRoomVec = new Coordinate(_chunkSize - 1, i);
                                } else {
                                    roomVec = new Coordinate(_chunkSize - 1, i);
                                    adjRoomVec = new Coordinate(0, i);
                                }

                                var room = chunk.Rooms[(int) roomVec.X, (int) roomVec.Y] as ProtoRoom;
                                var adjRoom = adjChunk.Rooms[(int) adjRoomVec.X, (int) adjRoomVec.Y] as ProtoRoom;

                                if (room?.SideStates[dir] != adjRoom?.SideStates[Utility.GetOppositeDirection(dir)]) {
                                    var open = (DateTime.Now.Ticks & 1) == 1;

                                    room?.SetSideState(dir, open);
                                    adjRoom?.SetSideState(Utility.GetOppositeDirection(dir), open);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}