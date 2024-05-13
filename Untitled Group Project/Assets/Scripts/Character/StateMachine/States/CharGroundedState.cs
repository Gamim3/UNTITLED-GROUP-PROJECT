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
        Ctx.MoveMultiplier = 1f;
        Ctx.ForceSlowDownRate = 5;
        Ctx.IsAired = false;
        Ctx.DesiredMoveForce = Ctx.MoveSpeed;
        Ctx.IsJumpTime = Ctx.MaxJumpTime;
        Ctx.JumpMent = new Vector3(0, 1, 0);

        Ctx.JumpAmount = 1; // idk yet

        if (Ctx.MoveForce < Ctx.MoveSpeed)
        {
            Ctx.MoveForce = Ctx.MoveSpeed;
        }
    }

    public override void ExitState() { }

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