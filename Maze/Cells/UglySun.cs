using Maze.Cells.Creatures;
using Maze.Cells.Creatures.Interfaces;
using Maze.LevelStaff;

namespace Maze.Cells
{
    internal class UglySun : BaseCell
    {
        private const int _GIVE_STRESS = 5;
        public UglySun(int coordinateX, int coordinateY, ILevel level, ConsoleColor color = ConsoleColor.DarkRed) : base(coordinateX, coordinateY, level, color)
        { }

        public override string Symbol => "*";

        public override bool Step(IBaseCreature creature)
        {
            var hero = creature as Hero;
            if (hero is not null)
            {
                if (hero.Stress + _GIVE_STRESS <= Hero.MAX_HERO_STRESS)
                {
                    hero.Stress += _GIVE_STRESS;
                }
                else
                {
                    hero.Stress = Hero.MAX_HERO_STRESS;
                }

                var ground = new Ground(CoordinateX, CoordinateY, Level);
                Level.ReplaceCell(this, ground);
            }

            return true;
        }
    }
}