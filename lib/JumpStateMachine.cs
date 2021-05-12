using System;

class JumpStateMachine
{
  enum State
  {
    READY_TO_JUMP,
    APPLYING_FORCE,
    IN_AIR,
    LANDED
  }

  public int MaxJumpForceApplications = 7;
  public int JumpForce = -2000;
  public int JumpReductionFactor = 400;
  public int JumpCooldown = 10;

  private State _state = State.READY_TO_JUMP;
  private int _jumpCount = 0;
  private int _jumpCooldown = 0;

  public float GetVelocityChange(bool jumpButtonPressed, float currentVelocity, bool isOnFloor, bool isOnCeiling)
  {
    switch (_state)
    {
      case State.READY_TO_JUMP:
        return ReadyHandler(jumpButtonPressed);
      case State.APPLYING_FORCE:
        return ApplyingForceHandler(jumpButtonPressed, currentVelocity, isOnFloor, isOnCeiling);
      case State.IN_AIR:
        return InAirHandler(currentVelocity, isOnFloor, isOnCeiling);
      case State.LANDED:
        return LandedHandler();
      default:
        return 0;
    }
  }

  private float ReadyHandler(bool jumpButtonPressed)
  {
    if (jumpButtonPressed)
    {
      _jumpCooldown = JumpCooldown;
      _state = State.APPLYING_FORCE;
      return JumpForce;
    }
    return 0;
  }

  private float ApplyingForceHandler(bool jumpButtonPressed, float currentVelocity, bool isOnFloor, bool isOnCeiling)
  {
    if (isOnFloor) _state = State.LANDED;
    if (!jumpButtonPressed || _jumpCount >= MaxJumpForceApplications) _state = State.IN_AIR;

    if (isOnCeiling)
    {
      _state = State.IN_AIR;
      return -currentVelocity + 2;
    }

    if (jumpButtonPressed && _jumpCount++ < MaxJumpForceApplications)
    {
      return JumpForce + (_jumpCount * JumpReductionFactor);
    }
    return 0;
  }

  private float InAirHandler(float currentVelocity, bool isOnFloor, bool isOnCeiling)
  {
    if (isOnFloor) _state = State.LANDED;
    if (isOnCeiling)
      return -currentVelocity + 2;

    return 0;
  }

  private float LandedHandler()
  {
    _jumpCount = 0;
    if (--_jumpCooldown == 0) _state = State.READY_TO_JUMP;
    return 0;
  }

}