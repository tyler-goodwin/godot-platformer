using Godot;
using System;


public class Spell : Area2D
{

  public enum Direction
  {
    LEFT, RIGHT
  }
  // Declare member variables here. Examples:
  // private int a = 2;
  // private string b = "text";
  [Export] public float Speed = 1000;

  public Direction TravelDirection { get; set; }

  private bool _dying = false;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {

  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //
  //  }

  public override void _Process(float delta)
  {
    if (_dying) return;

    var velocity = new Vector2();
    if (TravelDirection == Direction.RIGHT)
    {
      velocity.x += Speed;
    }
    else
    {
      velocity.x -= Speed;
    }
    velocity.x *= delta;

    Position += velocity;
  }

  public void OnBodyEntered(PhysicsBody2D body)
  {
    _dying = true;

    if (body.IsInGroup("enemies"))
    {
      //
    }
    var sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    sprite.Play("collision");
    sprite.Connect("animation_finished", this, "Remove");
  }

  public void TravelRight()
  {
    TravelDirection = Direction.RIGHT;
  }

  public void TravelLeft()
  {
    TravelDirection = Direction.LEFT;
    GetNode<AnimatedSprite>("AnimatedSprite").FlipH = true;
  }

  public void Remove()
  {
    QueueFree();
  }
}
