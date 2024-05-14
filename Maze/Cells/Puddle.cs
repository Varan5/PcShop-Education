using Maze.Cells.Creatures.Interfaces;
using Maze.LevelStaff;

namespace Maze.Cells
{

    public class Puddle : BaseCell
    {
        public Puddle(int coordinateX, int coordinateY, Level level, ConsoleColor color = ConsoleColor.DarkBlue) : base(coordinateX, coordinateY, level, color)
        {
        }

        public override string Symbol => "O";

        public override bool Step(IBaseCreature creature)
        {
            return false;
        }
    }
}
