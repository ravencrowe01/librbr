using Raven.Pathfinding.Util.Config;
using Raven.Pathfinding.Util.Nodes;

namespace librbr.Framework.World {
    public class PathfinderConfig : IPathfinderConfig {
        #nullable disable
        public Node Start { get; set; }
        public Node End { get; set; }
        public Node[ , ] Map { get; set; }
        #nullable enable
    }
}