using UnityEngine;

public class CharJumpState : CharBaseState
{
    public CharJumpState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Jump";

        // IsRootState = true;
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

    public override void InitializeSubState()
    {
        if (!Ctx.IsMoveAction)
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
            // DASH STATE
            // SetSubState(Factory.Dash()); 
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
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SwitchState(Factory.Airborne());
        }
        // else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped) // Maybe for double jump check if has jumps left
        // {
        //     SwitchState(Factory.Jumping());
        // }
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
