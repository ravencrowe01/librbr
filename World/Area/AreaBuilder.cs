using System.Numerics;
using librbr.Framework;
using librbr.Framework.World;
using librbr.Framework.World.Area;
using librbr.Framework.World.Chunk;
using librbr.World.Chunk;

namespace librbr.World.Area
{
    public class AreaBuilder : IAreaBuilder {
        private int _size;

        private int _chunkSize;

        private IRNGProvider _rng;

        public IAreaBuilder WithSize(int size) {
            _size = size;
            return this;
        }

        public IAreaBuilder WithChunkSize(int size) {
            _chunkSize = size;
            return this;
        }

        public IAreaConfig BuildArea(int rng) {
            throw new NotImplementedException();
        }

        private void AddSeedChunks (int seed, ProtoArea area) {
            var a = _size * _size;
            _rng.SetSeed (seed);

            for (int i = 0; i < a / 10 + 1; i++) {
                int x, y;

                do {
                    x = _rng.Range (0, _size);
                    y = _rng.Range (0, _size);
                } while (area.GetChunk (x, y) is not null);

                var chunk = BuildChunk (seed, area, new Vector2 (x, y));

                area.SetChunk (chunk, x, y);
            }
        }

        private void AddChunks (int seed, ProtoArea area) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    if (area.GetChunk (x, y) is null) {
                        var chunk = BuildChunk (seed, area, new Vector2 (x, y));
                        area.SetChunk (chunk, x, y);
                    }
                }
            }
        }

        private IChunkConfig BuildChunk (int seed, ProtoArea area, Vector2 cords) {
            var builder = new ChunkBuilder ().WithDiminsions (_chunkSize).WithCoordinates (cords);

            foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                var nCords = cords + Utility.GetVectorFromDirection (dir);

                if (nCords.X >= 0 && nCords.X < _size && nCords.Y >= 0 && nCords.Y < _size) {
                    var neighbor = area.GetChunk ((int) nCords.X, (int) nCords.Y);

                    if (neighbor is not null) {
                        AddConnections (builder, dir, neighbor as ProtoChunk);
                    }
                }
            }

            return builder.BuildChunk (seed, null);
        }

        private void AddConnections (IChunkBuilder builder, Direction dir, ProtoChunk neighbor) {
            var opDir = Utility.GetOppositeDirection (dir);

            var open = neighbor.GetOpenSides ()[opDir];

            foreach (var op in open) {
                var target = GetConnectionTarget (op.Coordinates, dir);

                builder.WithConnection (target, dir);
            }
        }

        private Vector2 GetConnectionTarget (Vector2 origin, Direction dir) {
            if (dir == Direction.North) {
                return new Vector2 (origin.X, _chunkSize - 1);
            }

            if (dir == Direction.South) {
                return new Vector2 (origin.X, 0);
            }

            if (dir == Direction.East) {
                return new Vector2 (_chunkSize - 1, origin.Y);
            }

            if (dir == Direction.West) {
                return new Vector2 (0, origin.Y);
            }

            return new Vector2 (-1, -1);
        }

        private void FixChunkConnections (ProtoArea area) {
            for (int x = 0; x < _size; x++) {
                for (int y = 0; y < _size; y++) {
                    var chunk = area.GetChunk (x, y);

                    foreach (var dir in (Direction[]) Enum.GetValues (typeof (Direction))) {
                        var adjVec = Utility.GetVectorFromDirection (dir) + new Vector2 (x, y);

                        if (adjVec.X >= 0 && adjVec.X < _size && adjVec.Y >= 0 && adjVec.Y < _size) {
                            var adjChunk = area.GetChunk ((int) adjVec.X, (int) adjVec.Y);

                            for (int i = 0; i < _chunkSize; i++) {
                                Vector2 roomVec;
                                Vector2 adjRoomVec;

                                if (dir == Direction.North) {
                                    roomVec = new Vector2 (i, _chunkSize - 1);
                                    adjRoomVec = new Vector2 (i, 0);
                                }
                                else if (dir == Direction.South) {
                                    roomVec = new Vector2 (i, 0);
                                    adjRoomVec = new Vector2 (i, _chunkSize - 1);
                                }
                                else if (dir == Direction.East) {
                                    roomVec = new Vector2 (0, i);
                                    adjRoomVec = new Vector2 (_chunkSize - 1, i);
                                }
                                else {
                                    roomVec = new Vector2 (_chunkSize - 1, i);
                                    adjRoomVec = new Vector2 (0, i);
                                }

                                var room = chunk.Rooms[(int) roomVec.X, (int) roomVec.Y] as ProtoRoom;
                                var adjRoom = adjChunk.Rooms[(int) adjRoomVec.X, (int) adjRoomVec.Y] as ProtoRoom;

                                if (room.SideStates[dir] != adjRoom.SideStates[Utility.GetOppositeDirection (dir)]) {
                                    var open = (DateTime.Now.Ticks & 1) == 1;

                                    room.SetSideState (dir, open);
                                    adjRoom.SetSideState (Utility.GetOppositeDirection (dir), open);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}