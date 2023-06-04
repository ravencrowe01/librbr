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

            var map = BuildNodeMap(chunk);

            for (int x = 0; x < chunk.Size; x++) {
                for (int y = 0; y < chunk.Size; y++) {
                    var current = new Coordinate(x, y);

                    if (!traveled.Contains(current * 2) && current != chunk.Center) {

                        var finder = new AStarPathfinder(GeneratePFConfig(chunk, current * 2, map));
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
                        // Pre-optimization for arbitrarily large chunks.
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

        private IPathfinderConfig GeneratePFConfig (IChunkConfig chunk, Coordinate start, Node[,] map) {
            return new PathfinderConfig {
                Start = new Node(true, start),
                Target = new Node(true, chunk.Center * 2),
                Map = map
            };
        }

        private Node[ , ] BuildNodeMap (IChunkConfig chunk) {
            var size = chunk.Size * 2 - 1;
            var map = new Node[size, size];

            for (int x = 0; x < size; x++) {
                for (int y = 0; y < size; y++) {
                    if (x % 2 == 0 && y % 2 == 0) {
                        map[x, y] = new Node(true, new Coordinate(x, y));
                    }

                    else if (x % 2 == 1 && y % 2 == 1) {
                        map[x, y] = new Node(false, new Coordinate(x, y));
                    }

                    else {
                        if (x > 0 && x % 2 == 1) {
                            var xx = (int) MathF.Ceiling((float) x / 2f) - 1;
                            map[x, y] = new Node(chunk.Rooms[xx , y / 2].SideStates[Direction.East], new Coordinate(x, y));
                        }

                        if (y > 0 && y % 2 == 1) {
                            var yy = (int) MathF.Ceiling((float) y / 2f) - 1;
                            map[x, y] = new Node(chunk.Rooms[x / 2, yy].SideStates[Direction.North], new Coordinate(x, y));
                        }
                    }
                }
            }

            return map;
        }
    }
}