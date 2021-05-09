using Godot;
using System;
using Direction = Types.Direction;

public class Hero : KinematicBody2D
{
  static string ANIMATION = "Animation";
  static string SPELL_POSITION = "SpellStart";

  static class InputDirection
  {
    public const string RIGHT = "ui_right";
    public const string LEFT = "ui_left";
    public const string UP = "ui_up";
    public const string DOWN = "ui_down";
    public const string JUMP = "ui_accept";
    public const string ATTACK = "ui_attack";
  }

  static class Animation
  {
    public const string RUN = "run";
    public const string IDLE = "idle";
  }

  [Export] public PackedScene Spell;

  [Export] public float MoveSpeed = 1600;

  [Export] public float GravityForce = 100;
  [Export] public float MaxFallSpeed = 3500;

  [Export] public float JumpForce = 650;
  [Export] public int MaxJumpForceApplications = 7;

  private Vector2 _velocity = new Vector2();
  private Direction _direction = Direction.RIGHT;
  private int _jumpCount = 0;
  private int _jumpCooldown = 0;

  private JumpStateMachine _jumper = new JumpStateMachine();

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
    IsAttackPressed();
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
      _velocity.y = 0;

    if (_velocity.y < MaxFallSpeed)
      _velocity.y += GravityForce;
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
    var jumpButtonPressed = Input.IsActionPressed(InputDirection.JUMP);
    _velocity.y += _jumper.GetVelocityChange(
      jumpButtonPressed,
      _velocity.y,
      IsOnFloor(),
      IsOnCeiling()
    );
  }

  private void IsAttackPressed()
  {
    if (Input.IsActionJustPressed(InputDirection.ATTACK))
    {
      var spell = (Spell)Spell.Instance();
      var startLocation = GetNode<Position2D>(SPELL_POSITION).Position;

      if (_direction == Direction.RIGHT)
      {
        spell.TravelRight();
      }
      else
      {
        spell.TravelLeft();
        startLocation.x *= -1;
      }

      Owner.AddChild(spell);
      spell.Position = ToGlobal(startLocation);
    }
  }
}
