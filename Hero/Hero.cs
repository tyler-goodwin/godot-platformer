using Godot;
using System;

public class Hero : KinematicBody2D
{
  static string ANIMATION = "Animation";

  static class InputDirection
  {
    public const string RIGHT = "ui_right";
    public const string LEFT = "ui_left";
    public const string UP = "ui_up";
    public const string DOWN = "ui_down";
    public const string JUMP = "ui_accept";
  }

  static class Animation
  {
    public const string RUN = "run";
    public const string IDLE = "idle";
  }

  enum Direction
  {
    LEFT, RIGHT
  }

  [Export] public float MoveSpeed = 800;

  [Export] public float GravityForce = 100;
  [Export] public float MaxFallSpeed = 1500;

  [Export] public float JumpForce = 200;
  [Export] public int MaxJumpForceApplications = 8;

  private Vector2 _velocity = new Vector2();
  private Direction _direction = Direction.RIGHT;
  private int _jumpCount = 0;

  public override void _Ready()
  {
  }

  public override void _Process(float delta)
  {
    UpdateAnimation();
    UpdateDirection();
  }

  public override void _PhysicsProcess(float delta)
  {
    UpdateVelocity();
    DoGravity();
    JumpHandler();
    MoveAndSlide(_velocity, new Vector2(0, -1));
  }

  public void UpdateVelocity()
  {

    _velocity.x = 0;
    if (Input.IsActionPressed(InputDirection.RIGHT))
      _velocity.x += MoveSpeed;
    if (Input.IsActionPressed(InputDirection.LEFT))
      _velocity.x -= MoveSpeed;
  }

  public void UpdateAnimation()
  {
    if (Mathf.Abs(_velocity.x) > 0)
      SetAnimation(Animation.RUN);
    else
      SetAnimation(Animation.IDLE);
  }

  private void UpdateDirection()
  {
    if (_velocity.x < 0)
      _direction = Direction.LEFT;
    if (_velocity.x > 0)
      _direction = Direction.RIGHT;

    GetNode<AnimatedSprite>(ANIMATION).FlipH = _direction != Direction.RIGHT;
  }

  private void DoGravity()
  {
    if (IsOnFloor())
    {
      _velocity.y = 0;
      return;
    }

    if (_velocity.y < MaxFallSpeed)
    {
      _velocity.y += GravityForce;
    }
  }

  private void SetAnimation(string animation)
  {
    var sprite = GetNode<AnimatedSprite>(ANIMATION);
    if (sprite.Animation != animation)
    {
      sprite.Animation = animation;
    }
  }

  private void JumpHandler()
  {
    if (IsOnFloor()) _jumpCount = 0;

    if (Input.IsActionPressed(InputDirection.JUMP))
    {
      if (_jumpCount++ < MaxJumpForceApplications)
        _velocity.y -= JumpForce;
    }
  }
}
