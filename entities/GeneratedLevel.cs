using Godot;
using System;
using static MazeGenerator.Directions;

public class GeneratedLevel : Node
{
    const float GRID_WIDTH = 1600f;

    [Export] public PackedScene Floor;

    private LevelGenerator _generator;
    private Hero _hero;
    private Random _random;

    public override void _Ready()
    {
        this._random = new Random();
        this._hero = GetNode<Hero>("Hero");
        this._generator = new LevelGenerator(this.MazeOpts());
        this._generator.Generate();
        this.AddWallsAndFloors();
    }

    private void AddWallsAndFloors() {
        Direction[] floorDirections =  { Direction.NORTH, Direction.SOUTH };
        foreach(var cell in this._generator.Maze.Cells) {
            foreach(var direction in floorDirections) {
                bool isCeiling = direction == Direction.NORTH;
                AddFloor(cell.Coords, cell.GetWall(direction), isCeiling);
            }
        }
    }

    private void AddFloor((int, int) coords, MazeGenerator.Wall wall, bool isCeiling) {
        if(wall == null || !wall.Present) return;

        // Remove wall so it won't be added twice
        wall.Present = false;

        var floor = (FloorTile) Floor.Instance();
        AddChild(floor);
        RandomizeFloorVisuals(floor);
        UpdateFloorPosition(coords, floor, isCeiling);
    }

    private void UpdateFloorPosition((int, int) coords, FloorTile floor, bool isCeiling) {
        var (cellXOffset, cellYOffset) = coords;
        var yOffset = isCeiling ? (GRID_WIDTH / 2) : -(GRID_WIDTH/2);
        floor.Position = new Vector2(cellXOffset * GRID_WIDTH, cellYOffset * GRID_WIDTH + yOffset);
    }

    private void RandomizeFloorVisuals(FloorTile floor) {
        floor.Foliage = RandomBool();
        floor.Stalactites = RandomBool();
        floor.Rocks = RandomBool();
        floor.UpdateVisibleSprites();
    }

    private bool RandomBool() {
        return this._random.Next(0, 2) == 0;
    }

    private LevelGenerator.MazeOpts MazeOpts() {
        var mazeOpts = new LevelGenerator.MazeOpts();
        mazeOpts.Height = 8;
        mazeOpts.Width = 12;
        mazeOpts.Seed = 0; // For Random
        mazeOpts.HorizontalBias = 0.75f;
        return mazeOpts;
    }
}
