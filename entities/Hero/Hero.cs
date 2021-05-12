using Godot;
using System;
using Direction = Types.Direction;

public class Hero : KinematicBody2D
{
  static string ANIMATION = "Animation";
  static string SPELL_POSITION = "SpellStart";
  static string EFFECTS_PLAYER = "EffectsPlayer";
  static string DAMAGE_COOLDOWN_TIMER = "DamageCooldownTimer";

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

  static class Effects {
    public const string DAMAGE_TAKEN = "DamageTaken";
  }

  [Export] public PackedScene Spell;

  [Export] public float MoveSpeed = 1600;

  [Export] public float GravityForce = 100;
  [Export] public float MaxFallSpeed = 3500;

  [Export] public int MaxHealth = 10;
  [Export] public float DamageCooldown = 1.0F;

  [Signal]
  public delegate void HealthChanged(int newValue);

  private Vector2 _velocity = new Vector2();
  private Direction _direction = Direction.RIGHT;

  private int _currentHealth = 10;
  private bool _canTakeDamage = true;

  private JumpStateMachine _jumper = new JumpStateMachine();

  public override void _Ready()
  {
    GetNode<Timer>(DAMAGE_COOLDOWN_TIMER).Connect("timeout", this, "EnableTakingDamage");
    _currentHealth = MaxHealth;
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

    var totalCollisions = GetSlideCount();
    for(var i = 0; i < totalCollisions; ++i) {
      var collision = GetSlideCollision(i);
      if(collision.Collider.IsClass("KinematicBody2D"))
      {
        var collider = (KinematicBody2D) collision.Collider;
        if(collider.IsInGroup("enemies")) {
          TakeDamage();
        }
      }
    }
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

  public void EnableTakingDamage() {
    _canTakeDamage = true;
  }

  private void TakeDamage() {
    if(_canTakeDamage) {
      _canTakeDamage = false;
      UpdateHealth(_currentHealth - 1);
      GetNode<Timer>(DAMAGE_COOLDOWN_TIMER).Start();
      GetNode<AnimationPlayer>(EFFECTS_PLAYER).Play(Effects.DAMAGE_TAKEN);

      if(_currentHealth < 0) {
        GD.Print("DEAD!");
      }
    }
  }

  private void UpdateHealth(int newValue) {
    _currentHealth = newValue;
    EmitSignal(nameof(HealthChanged), newValue);
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
