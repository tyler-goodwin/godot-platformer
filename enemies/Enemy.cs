using Godot;
using System;

using Direction = Types.Direction;

public class Enemy : KinematicBody2D
{
  [Export] public float Speed = 350;
  [Export] public float GravityStrength = 500;

  protected Direction _direction = Types.Direction.LEFT;
  protected bool _dying = false;

  public override void _Process(float delta)
  {
    if (_dying) return;

    var velocity = new Vector2(0, GravityStrength);
    if (_direction == Direction.RIGHT)
    {
      velocity.x += Speed;
    }
    else
    {
      velocity.x -= Speed;
    }
    MoveAndSlide(velocity);
  }

  public void Die()
  {
    _dying = true;
    QueueFree();
  }

  public void ChangeDirection()
  {
    _direction = _direction == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
  }
}