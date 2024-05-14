using Maze.Cells.Creatures.Interfaces;
using Maze.LevelStaff;

namespace Maze.Cells
{
    public  class Gold : BaseCell
    {
        public Gold(int coordinateX, int coordinateY, Level level) : base(coordinateX, coordinateY, level)
        {

        }








        public override string Symbol => "+";
        public override bool Step(IBaseCreature creature)
        {
            return true;
        }
    }
}
