using librbr.Framework.World;
using librbr.Framework.World.Room;
using Raven.Util;

namespace librbr.World {
    public class ProtoRoom : IRoomConfig
    {
        public IReadOnlyDictionary<Direction, bool> SideStates => _sideStates;
        private Dictionary<Direction, bool> _sideStates = new Dictionary<Direction, bool>{
            { Direction.North, false },
            { Direction.South, false },
            { Direction.East, false },
            { Direction.West, false }
        };

        public Coordinate Coordinates { get; set; }

        public IEnumerable<Direction> GetOpenSides() => SideStates.Keys.Where(d => SideStates[d]);

        public void SetSideState(Direction dir, bool state) => _sideStates[dir] = state;

        public void OpenSide(Direction dir) => SetSideState(dir, true);

        public void CloseSide(Direction dir) => SetSideState(dir, false);
    }
}