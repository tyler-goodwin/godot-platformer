using Godot;
using System;
using Direction = Types.Direction;

public class Spell : Area2D
{

  [Export] public float Speed = 1000;

  public Direction TravelDirection { get; set; }

  private bool _dying = false;

  public override void _Ready()
  { }

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
      ((Enemy)body).Die();
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
