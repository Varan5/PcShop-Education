using Maze.Cells.Creatures.Interfaces;
using Maze.LevelStaff;

namespace Maze.Cells
{
    internal class Trap : BaseCell
    {
        public Trap(int coordinateX, int coordinateY, Level level) : base(coordinateX, coordinateY, level)
        {
        }

        public override string Symbol => "^";

        public override bool Step(IBaseCreature creature)
        {
            return true;
        }
    }
}
