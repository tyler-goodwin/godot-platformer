using NUnit.Framework;

namespace MazeGenerator.Tests {
  [TestFixture]
  public class MazeTest {
    private Maze maze;

    [SetUp]
    public void Setup() {
      maze = new Maze(5, 4);
    }

    [Test]
    public void CellsWereCreated() {
      int count = 0;
      foreach(var cell in maze.Cells) {
        Assert.That(cell, Is.Not.Null);
        ++count;
      }
      Assert.That(count, Is.EqualTo(20));
    }
  }
}