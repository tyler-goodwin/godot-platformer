namespace MazeGenerator {
  public class Maze {
    public Cell[,] Cells;

    private int _width;
    private int _height;

    public Maze(int width, int height) {
      _width = width;
      _height = height;

      Cells = new Cell[_width, _height];
      InitializeMaze();
    }

    public Cell GetCell((int, int) coords) {
      if(!InBounds(coords)) return null;

      var (x, y) = coords;
      return Cells[x, y];
    }

    public bool InBounds((int, int) coords) {
      var (x, y) = coords;
      if(x < 0 || x >= _width) return false;
      if(y < 0 || y >= _height) return false;

      return true;
    }

    public void RemoveWall((int, int) coords, Directions.Direction direction) {
      var (x, y) = coords;
      if(!InBounds(coords)) throw new System.Exception($"Out of bounds, {x}, {y}");

      var wall = GetCell(coords).GetWall(direction);
      if(wall == null) throw new System.Exception($"No wall at {x}, {y} in direction {direction}");

      wall.Present = false;
    }

    private void InitializeMaze() {
      for(int h = 0; h < _height; ++h) {
        for(int w = 0; w < _width; ++w) {
          InitializeCell((w, h));
        }
      }
    }

    private void InitializeCell((int, int) coords) {
      var (x, y) = coords;
      var cell = new Cell(x, y);
      var neighbourCoords = cell.GetNeighbourCoords();

      foreach(var direction in Directions.AllDirections) {
        var neighbour = GetCell(neighbourCoords[(int) direction]);
        if(neighbour == null) {
          continue;
        }

        // This will result in some walls being overriden, but I don't
        // think we don't really care
        var wall = new Wall();
        cell.SetWall(wall, direction);
        neighbour.SetWall(wall, Directions.Opposite(direction));
      }

      Cells[x, y] = cell;
    }
  }
}