using Raven.Pathfinding.Framework.Config;
using Raven.Pathfinding.Util.Nodes;

namespace librbr.World.Config {
    public class PathfinderConfig : IPathfinderConfig {
        public Node Start { get; set; }
        public Node Target { get; set; }
        public Node[ , ] Map { get; set; }
    }
}