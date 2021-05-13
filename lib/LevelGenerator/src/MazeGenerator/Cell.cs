namespace MazeGenerator {
  using static Directions;
  public class Cell {

    public (int, int) Coords;

    public Wall[] Walls = { null, null, null, null };

    public Cell(int x, int y) {
      Coords = (x, y);
    }

    public int GetX() { return Coords.Item1; }
    public int GetY() { return Coords.Item2; }

    public (int, int)[] GetNeighbourCoords() {
      return new (int, int)[] {
        Directions.Move(Coords, Direction.NORTH),
        Directions.Move(Coords, Direction.EAST),
        Directions.Move(Coords, Direction.SOUTH),
        Directions.Move(Coords, Direction.WEST),
      };
    }

    public override string ToString()
    {
      var (x, y) = Coords;
      return $"{x}, {y} Walls: [N: {GetWall(Direction.NORTH)}, E: {GetWall(Direction.EAST)}, S: {GetWall(Direction.SOUTH)}, W: {GetWall(Direction.WEST)}]";
    }

    public Wall GetWall(Direction direction) {
      return Walls[(int) direction];
    }

    public void SetWall(Wall wall, Direction direction) {
      Walls[(int) direction] = wall;
    }
  }
}