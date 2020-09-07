using Godot;
using System;

public class ShroomEnemy : Enemy
{
  public void OnChangeDirectionTimerTimeout()
  {
    ChangeDirection();
    var animation = GetNode<AnimatedSprite>("AnimatedSprite");
    if (_direction == Types.Direction.LEFT)
    {
      animation.FlipH = false;
    }
    else
    {
      animation.FlipH = true;
    }
  }
}
