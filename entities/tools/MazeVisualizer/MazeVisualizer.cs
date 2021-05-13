using Godot;
using MazeGenerator;
using static MazeGenerator.Directions;

public class MazeVisualizer : Node2D
{
    [Export] public float CellWidth = 100f;
    [Export] public int Width = 6;
    [Export] public int Height = 8;
    [Export] public float WallWidth = 5f;

    private Maze _maze;

    public override void _Ready()
    {
        var generator = new MazeGenerator.Generator(Width, Height, (0, 3));
        _maze = generator.Generate();
    }

    public override void _Draw()
    {
        var radius = CellWidth * 0.5f;
        var red = new Color(1.0f, 0, 0);
        var offset = CellWidth * 0.5f;

        DrawRect(new Rect2(0, 0, Width * CellWidth, Height * CellWidth), new Color(0, 0, 0));

        foreach(var cell in _maze.Cells) {
            foreach(var direction in AllDirections) {
                var wall = cell.GetWall(direction);
                if(wall == null || !wall.Present) continue;

                DrawWall(direction, cell.Coords);
            }
        }
    }

    public override void _Process(float delta)
    {

    }

    private void DrawWall(Direction direction, (int, int) coords) {
        var (x, y) = coords;
        var white = new Color(1f, 1f, 1f);
        var start = new Vector2(x * CellWidth, y * CellWidth) + StartOffset(direction);
        var end = start + EndOffset(direction);
        DrawLine(start, end, white, WallWidth);
    }

    private Vector2 StartOffset(Direction direction) {
        switch(direction) {
            case Direction.NORTH:
            case Direction.WEST:
                return new Vector2(0f, 0f);
            case Direction.EAST:
                return new Vector2(CellWidth, 0f);
            case Direction.SOUTH:
                return new Vector2(0f, CellWidth);
        }
        throw new System.Exception("Should not have hit this code");
    }

    private Vector2 EndOffset(Direction direction) {
        switch(direction) {
            case Direction.NORTH:
            case Direction.SOUTH:
                return new Vector2(CellWidth, 0);
            case Direction.EAST:
            case Direction.WEST:
                return new Vector2(0, CellWidth);
        }
        throw new System.Exception("Should not have hit this code");
    }
}
