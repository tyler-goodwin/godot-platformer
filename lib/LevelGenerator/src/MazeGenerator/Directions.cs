namespace MazeGenerator {
  public static class Directions {
    public enum Direction: int {
      NORTH = 0,
      EAST = 1,
      SOUTH = 2,
      WEST = 3,
    };

    public static Direction[] AllDirections = { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST };

    public static (int, int) Delta(Direction direction) {
      switch(direction) {
        case Direction.NORTH:
          return (0, -1);
        case Direction.EAST:
          return (1, 0);
        case Direction.SOUTH:
          return (0, 1);
        case Direction.WEST:
          return (-1, 0);
      }
      throw new System.Exception("Should not happen");
    }

    public static (int, int) Move((int, int) start, Direction direction) {
      var (x, y) = start;
      var (dx, dy) = Delta(direction);
      return (x + dx, y + dy);
    }

    public static Direction Opposite(Direction direction) {
      switch(direction) {
        case Direction.NORTH:
          return Direction.SOUTH;
        case Direction.EAST:
          return Direction.WEST;
        case Direction.SOUTH:
          return Direction.NORTH;
        case Direction.WEST:
          return Direction.EAST;
      }
      throw new System.Exception("Should not happen");
    }
  }
}