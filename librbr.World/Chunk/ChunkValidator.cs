using librbr.Framework.World;
using librbr.Framework.World.Chunk;
using Raven.Pathfinding.AStar;
using Raven.Pathfinding.Framework;
using Raven.Pathfinding.Framework.Config;
using Raven.Pathfinding.Framework.Nodes;
using Raven.Util;

namespace librbr.World.Chunk {
    public class ChunkValidator : IChunkValidator {
        public bool ValidateChunk (IChunkConfig chunk) {
            var traveled = new List<Coordinate>();

            for (int x = 0; x < chunk.Size; x++) {
                for (int y = 0; y < chunk.Size; y++) {

                    if (!traveled.Contains(new Coordinate(x, y)) && !(x == 0 && y == 0)) {

                        var finder = new AStarPathfinder(GeneratePFConfig(chunk, new Coordinate(x, y)));
                        var status = PathfindingStatus.Searching;

                        do {
                            status = finder.Step();
                        } while (status == PathfindingStatus.Searching);

                        if (status == PathfindingStatus.Invalid) {
                            return false;
                        }

                        var path = finder.BuildPath();
                        // Add all of the nodes we passed through to get to the center so that 
                        // traversed nodes can be skipped, as they are already valid.
                        foreach (var node in path) {
                            // Looks like I did need to override it.
                            if (!traveled.Contains(node.Coordinates)) {
                                traveled.Add(node.Coordinates);
                            }
                        }
                    }
                }
            }

            return true;
        }

        private IPathfinderConfig GeneratePFConfig (IChunkConfig chunk, Coordinate start) {
            return new PathfinderConfig {
                Start = new Node(true, start),
                Target = new Node(true, chunk.Center),
                Map = BuildNodeMap(chunk)
            };
        }

        private Node[ , ] BuildNodeMap (IChunkConfig chunk) {
            var size = chunk.Size * 2 - 1;
            var map = new Node[size, size];

            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (x % 2 == 0 && y % 2 == 0) {
                        map[x, y] = new Node(true, new Coordinate(x, y));

                        if (x < size - 2) {
                            map[x + 1, y] = new Node(chunk.Rooms[x / 2 + 1, y / 2].SideStates[Direction.East], new Coordinate(x / 2 + 1, y / 2));
                        }

                        if (y < size - 2) {
                            map[x, y + 1] = new Node(chunk.Rooms[x / 2, y / 2 + 1].SideStates[Direction.North], new Coordinate(x / 2, y / 2 + 1));
                        }
                    }

                    if (x % 2 == 1 && y % 2 == 1) {
                        map[x, y] = new Node(false, new Coordinate(x, y));
                    }
                }
            }

            return map;
        }
    }
}