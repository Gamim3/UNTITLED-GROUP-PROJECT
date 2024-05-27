using UnityEngine;
public class CharGroundedState : CharBaseState
{
    public CharGroundedState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsGroundedState = true;

        Ctx.MoveMultiplier = 1f;
        Ctx.ForceSlowDownRate = 5;
        Ctx.IsAirborneState = false;
        Ctx.DesiredMoveForce = Ctx.WalkSpeed;
        Ctx.IsJumpTime = Ctx.MaxJumpTime;
        Ctx.JumpMent = new Vector3(0, 1, 0);

        Ctx.JumpAmount = 1; // idk yet

        if (Ctx.MoveForce < Ctx.WalkSpeed)
        {
            Ctx.MoveForce = Ctx.WalkSpeed;
        }
    }

    public override void ExitState()
    {
        Ctx.IsGroundedState = false;
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
        if (!Ctx.IsMoveAction)
        {
            SetSubState(Factory.Idle());
        }
        if (Ctx.IsMoveAction)
        {
            SetSubState(Factory.Walking());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsJumpAction)
        {
            SwitchState(Factory.Jumping());
        }
    }
}