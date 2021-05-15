using System;
using System.Linq;
using System.Collections.Generic;
namespace MazeGenerator {

  using static Directions;


  public class Generator {
    private Maze _maze;
    private (int, int) _start;
    private Random _random;
    private float _horizontalBias;
    private Stack<Cell> _stack = new Stack<Cell>();
    private List<Cell> _visted = new List<Cell>();

    private static int NUM_DIRECTIONS = Enum.GetValues(typeof(Direction)).Length;

    public Generator(int width, int height, (int, int) start, int seed = 0, float horizontalBias = 0f) {
      _maze = new Maze(width, height);
      _random = seed != 0 ? new Random(seed) : new Random();
      _horizontalBias = horizontalBias;

      if(!_maze.InBounds(start)) throw new System.Exception("Invalid start location");
    }

    public Maze Generate() {
      var cell = _maze.GetCell(_start);
      _visted.Add(cell);
      _stack.Push(cell);

      while(_stack.Count > 0) {
        cell = _stack.Pop();

        var (directionFromCell, chosenNeighbour) = RandomUnvistedNeighbour(cell);

        if(directionFromCell is Direction chosenDirection) {
          _stack.Push(cell);
          _maze.RemoveWall(cell.Coords, chosenDirection);
          _stack.Push(chosenNeighbour);
          _visted.Add(chosenNeighbour);
        }
      }

      return _maze;
    }

    private Direction[] shuffledDirections() {
      List<Direction> directions = new List<Direction>(NUM_DIRECTIONS);
      List<Direction> list = new List<Direction>(Directions.AllDirections);

      // Shuffle
      var iterations = list.Count;
      for(var i = 0; i < iterations; ++i) {
        var selected = list[_random.Next(0, list.Count)];
        list.Remove(selected);
        directions.Add(selected);
      }

      // Use bias if should
      var shouldUseHorizontal = _random.NextDouble() < _horizontalBias;
      if(shouldUseHorizontal) {
        directions.Sort(CompareWithHorizontalBias);
      }

      return directions.ToArray();
    }

    private (Direction?, Cell) RandomUnvistedNeighbour(Cell cell) {
      var directions = shuffledDirections();

      // Choose random unvisited neighbour
      for(int i = 0; i < AllDirections.Length; ++i) {
        var chosenCoords = Move(cell.Coords, directions[i]);
        var neighbour = _maze.GetCell(chosenCoords);

        if(neighbour == null) continue;

        if(!_visted.Contains(neighbour)) {
          return (directions[i], neighbour);
        }
      }

      return (null, null);
    }

    private static int CompareWithHorizontalBias(Direction a, Direction b) {
      if(IsHorizontal(a) && IsHorizontal(b)) {
        return 0;
      }
      return IsHorizontal(a) ? -1 : 1;
    }

    private static bool IsHorizontal(Direction direction) {
      return direction == Direction.EAST || direction == Direction.WEST;
    }
  }
}