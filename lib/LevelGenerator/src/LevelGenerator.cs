using MazeGenerator;
public class LevelGenerator {
  public struct MazeOpts {
    public int Width;
    public int Height;
    public int Seed;
    public float HorizontalBias;
  }

  public Maze Maze;

  private MazeGenerator.Generator _mazeGenerator;

  public LevelGenerator(MazeOpts mazeOpts) {
    var start = (0, 0);
    this._mazeGenerator = new MazeGenerator.Generator(
      mazeOpts.Width,
      mazeOpts.Height,
      start,
      mazeOpts.Seed,
      mazeOpts.HorizontalBias
    );
  }

  public void Generate() {
    Maze = this._mazeGenerator.Generate();
  }
}