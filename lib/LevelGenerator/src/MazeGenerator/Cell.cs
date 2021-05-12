namespace MazeGenerator {
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
        Directions.Move(Coords, Directions.NORTH),
        Directions.Move(Coords, Directions.EAST),
        Directions.Move(Coords, Directions.SOUTH),
        Directions.Move(Coords, Directions.WEST),
      };
    }
  }
}