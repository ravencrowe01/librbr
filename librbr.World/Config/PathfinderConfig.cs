using Raven.Pathfinding.Framework.Config;
using Raven.Pathfinding.Framework.Nodes;

namespace librbr.World.Config {
#pragma warning disable CS8618
        public class PathfinderConfig : IPathfinderConfig {
            public Node Start { get; set; }
            public Node Target { get; set; }
            public Node[ , ] Map { get; set; }
        }
#pragma warning restore CS8618
}