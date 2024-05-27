using UnityEngine;

public class CharJumpState : CharBaseState
{
    public CharJumpState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsJumpingState = true;

        Ctx.IsExitingSlope = true;

        HandleJump();
    }

    public override void ExitState()
    {
        Ctx.IsJumpingState = false;
        Ctx.IsForced = false;
        Ctx.IsExitingSlope = false;
    }

    #region MonoBehaveiours

    public override void UpdateState() { }

    public override void FixedUpdateState()
    {
        CheckSwitchStates();
        HandleJumpTime();
    }

    #endregion

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    void HandleJump()
    {
        Ctx.IsForced = true;
        Ctx.ExtraForce = 21;

        Ctx.PlayerRigidBody.velocity = new Vector3(Ctx.PlayerRigidBody.velocity.x, 0f, Ctx.PlayerRigidBody.velocity.z);
        Ctx.PlayerRigidBody.AddForce(Ctx.JumpMent * Ctx.JumpForce, ForceMode.Impulse);
    }

    void HandleJumpTime()
    {
        if (Ctx.IsJumpTime > 0)
        {
            Ctx.IsJumpTime -= Time.deltaTime;
        }
        else
        {
            Ctx.IsJumpTime = 0;
        }
    }
}
