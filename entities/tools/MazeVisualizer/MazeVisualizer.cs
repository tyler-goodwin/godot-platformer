using Godot;
using MazeGenerator;

public class MazeVisualizer : Node2D
{
    [Export] public float CellWidth = 100f;
    [Export] public int Width = 6;
    [Export] public int Height = 8;
    [Export] public float WallWidth = 5f;

    private Maze _maze;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _maze = new Maze(Width, Height);
        _maze.RemoveWall((0, 1), Directions.EAST);
        _maze.RemoveWall((2, 3), Directions.SOUTH);
    }

    public override void _Draw()
    {
        var radius = CellWidth * 0.5f;
        var red = new Color(1.0f, 0, 0);
        var offset = CellWidth * 0.5f;

        DrawRect(new Rect2(0, 0, Width * CellWidth, Height * CellWidth), new Color(0, 0, 0));

        foreach(var cell in _maze.Cells) {
            foreach(var direction in Directions.AllDirections) {
                var wall = cell.Walls[direction];
                if(wall == null || !wall.Present) continue;

                DrawWall(direction, cell.Coords);
            }
        }
    }

    public override void _Process(float delta)
    {

    }

    private void DrawWall(int direction, (int, int) coords) {
        var (x, y) = coords;
        var white = new Color(1f, 1f, 1f);
        switch(direction) {
            case Directions.NORTH: {
                var start = new Vector2(x * CellWidth, y * CellWidth);
                var end = start + new Vector2(CellWidth, 0);
                DrawLine(start, end, white, WallWidth);
                return;
            }
            case Directions.SOUTH: {
                var start = new Vector2(x * CellWidth, y * CellWidth + CellWidth);
                var end = start + new Vector2(CellWidth, 0);
                DrawLine(start, end, white);
                return;
            }
            case Directions.EAST: {
                var start = new Vector2(x * CellWidth + CellWidth, y * CellWidth);
                var end = start + new Vector2(0, CellWidth);
                DrawLine(start, end, white, WallWidth);
                return;
            }
            case Directions.WEST: {
                var start = new Vector2(x * CellWidth, y * CellWidth);
                var end = start + new Vector2(0, CellWidth);
                DrawLine(start, end, white, WallWidth);
                return;
            }
            default:
                throw new System.Exception($"Woah there. We don't know what direction {direction} refers to");
        }
    }
}
