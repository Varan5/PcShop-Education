﻿using Maze.Cells.Creatures.Interfaces;
using Maze.LevelStaff;

namespace Maze.Cells
{
    public class Berry : BaseCell
    {
        public Berry(int coordinateX, int coordinateY, Level level) : base(coordinateX, coordinateY, level)
        {
        }

        public override string Symbol => "@";

        public override bool Step(IBaseCreature creature)
        {
            creature.Hp++;
            return true;
        }
    }
}
