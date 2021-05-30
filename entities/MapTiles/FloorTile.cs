using Godot;

public class FloorTile : StaticBody2D
{

    [Export] public bool Foliage = false;
    [Export] public bool Rocks = false;
    [Export] public bool Stalactites = false;

    public override void _Ready()
    {
        UpdateVisibleSprites();
    }

    public void UpdateVisibleSprites() {
        GetNode<Sprite>("foliage").Visible = Foliage;
        GetNode<Sprite>("rocks").Visible = Rocks;
        GetNode<Sprite>("stalactites").Visible = Stalactites;
    }
}
