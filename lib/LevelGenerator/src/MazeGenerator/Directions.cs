namespace MazeGenerator {
  public static class Directions {
    public const int NORTH = 0;
    public const int EAST = 1;
    public const int SOUTH = 2;
    public const int WEST = 3;

    public static int[] AllDirections = { NORTH, EAST, SOUTH, WEST };

    private static (int, int)[] deltas = {
      (0, 1), // North
      (1, 0), // East
      (0, -1), // South
      (-1, 0), // West
    };


    public static (int, int) Delta(int direction) {
      return deltas[direction];
    }

    public static (int, int) Move((int, int) start, int direction) {
      if(direction < 0 || direction > 3) {
        throw new System.ArgumentException($"Invalid direction {direction}");
      }

      var (x, y) = start;
      var (dx, dy) = deltas[direction];
      return (x + dx, y + dy);
    }

    public static int Opposite(int direction) {
      switch(direction) {
        case NORTH:
          return SOUTH;
        case EAST:
          return WEST;
        case SOUTH:
          return NORTH;
        case WEST:
          return EAST;
        default:
          // North is the opposite of everything right?
          return NORTH;
      }
    }
  }
}