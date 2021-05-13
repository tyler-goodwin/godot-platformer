using System;
using System.Linq;
using System.Collections.Generic;
namespace MazeGenerator {

  using static Directions;
  public class Generator {
    private Maze maze;
    private (int, int) start;
    private Random random;
    private Stack<Cell> stack = new Stack<Cell>();
    private List<Cell> visted = new List<Cell>();

    public Generator(int width, int height, (int, int) start, int? seed = null) {
      maze = new Maze(width, height);
      random = seed is int s ? new Random(s) : new Random();

      if(!maze.InBounds(start)) throw new System.Exception("Invalid start location");
    }

    public Maze Generate() {
      var cell = maze.GetCell(start);
      visted.Add(cell);
      stack.Push(cell);

      while(stack.Count > 0) {
        cell = stack.Pop();

        var (directionFromCell, chosenNeighbour) = RandomUnvistedNeighbour(cell);

        if(directionFromCell is Direction chosenDirection) {
          stack.Push(cell);
          maze.RemoveWall(cell.Coords, chosenDirection);
          stack.Push(chosenNeighbour);
          visted.Add(chosenNeighbour);
        }
      }

      return maze;
    }

    private static Direction[] shuffledDirections(Random random) {
      return Directions.AllDirections.OrderBy(x => random.Next()).ToArray();
    }

    private (Direction?, Cell) RandomUnvistedNeighbour(Cell cell) {
      var directions = shuffledDirections(random);

      // Choose random unvisited neighbour
      for(int i = 0; i < AllDirections.Length; ++i) {
        var chosenCoords = Move(cell.Coords, directions[i]);
        var neighbour = maze.GetCell(chosenCoords);

        if(neighbour == null) continue;

        if(!visted.Contains(neighbour)) {
          return (directions[i], neighbour);
        }
      }

      return (null, null);
    }

  }
}