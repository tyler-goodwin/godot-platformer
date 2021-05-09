using Godot;
using System;

using Direction = Types.Direction;

public class Enemy : KinematicBody2D
{
  [Export] public float Speed = 350;
  [Export] public float GravityStrength = 500;
  [Export] public int MaxHealth = 10;

  protected Direction _direction = Types.Direction.LEFT;
  protected bool _dying = false;
  protected int _currentHealth;

  public Enemy() {
    _currentHealth = MaxHealth;
  }

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

  public void Attacked(int damage) {
    _currentHealth -= damage;

    if(_currentHealth <= 0) {
      Die();
    }
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