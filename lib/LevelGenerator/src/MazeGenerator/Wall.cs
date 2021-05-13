namespace MazeGenerator {
  public class Wall {
    public bool Present = true;

    public override string ToString() {
      return Present ? "Y" : "N";
    }
  }
}