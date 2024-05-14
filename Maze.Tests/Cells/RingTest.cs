using Maze.Cells.Creatures.Interfaces;
using Maze.Cells;
using Maze.LevelStaff;
using Moq;
using NUnit.Framework;

namespace Maze.Tests.Cells
{
    public class RingTest
    {
        [Test]
        public void Ring_Symbol_ShouldBeEmptyIfUsed()
        {
            // Preparation
            var levelMock = new Mock<ILevel>();
            var creatureMock = new Mock<IHero>();

            var ring = new Ring(0, 0, levelMock.Object);

            // Action
            ring.Step(creatureMock.Object);

            // Assertion
            Assert.That(ring.Symbol, Is.EqualTo(" "), "Symbol should be empty after Ring is used");
        }
    }
}
