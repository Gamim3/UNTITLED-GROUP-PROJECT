using UnityEngine;

public class CharAirborneState : CharBaseState
{
    public CharAirborneState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Airborne";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsAirborneState = true;

        Ctx.MoveMultiplier = Ctx.AirSpeed;
        Ctx.ForceSlowDownRate = 1;
    }

    public override void ExitState()
    {
        Ctx.IsAirborneState = false;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
        Ctx.Movement = Ctx.CurrentMovement.normalized;
    }

    public override void FixedUpdateState()
    {
        CheckSwitchStates();
    }

    #endregion

    public override void InitializeSubState()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Running());
        }
        else if (Ctx.IsDashAction)
        {
            Debug.Log("Dash from Airborne");

            SetSubState(Factory.Dashing());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsSloped)
        {
            SwitchState(Factory.Sloped());
        }
        else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped)
        {
            SwitchState(Factory.Jumping());
        }
    }
}