using Raven.Pathfinding.Framework.Config;
using Raven.Pathfinding.Framework.Nodes;

namespace librbr.Framework.World {
    public class PathfinderConfig : IPathfinderConfig {
        #nullable disable
        public Node Start { get; set; }
        public Node Target { get; set; }
        public Node[ , ] Map { get; set; }
        #nullable enable
    }
}