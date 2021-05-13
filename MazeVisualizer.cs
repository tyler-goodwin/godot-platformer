using Godot;
using MazeGenerator;

public class MazeVisualizer : Node2D
{
    [Export] public float CellWidth = 100f;
    [Export] public int Width = 6;
    [Export] public int Height = 8;

    private Maze _maze;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _maze = new Maze(Width, Height);
    }

    public override void _Draw()
    {
        var radius = CellWidth * 0.5f;
        var red = new Color(1.0f, 0, 0);
        var offset = CellWidth * 0.5f;

        DrawRect(new Rect2(0, 0, Width * CellWidth, Height * CellWidth), new Color(0, 0, 0));

        foreach(var cell in _maze.Cells) {
            var (x, y) = cell.Coords;
            var position = new Vector2(x * CellWidth + offset , y * CellWidth + offset);
            DrawCircle(position, radius, red);
        }
    }

    public override void _Process(float delta)
    {

    }
}
