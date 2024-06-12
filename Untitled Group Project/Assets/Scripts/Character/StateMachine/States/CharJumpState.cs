using UnityEngine;

public class CharJumpState : CharBaseState
{
    public CharJumpState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Jump";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Debug.Log("Enter Jump");

        Ctx.IsJumpingState = true;

        Ctx.IsJumpTime = Ctx.MaxJumpTime;

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
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && Ctx.IsJumpTime == 0)
        {
            SetSubState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction && Ctx.IsJumpTime == 0)
        {
            SetSubState(Factory.Running());
        }
        else if (Ctx.IsDashAction)
        {
            SetSubState(Factory.Dashing());
        }
    }

    public override void CheckSwitchStates()
    {
        // Can only leave jump when jump time has reset
        if (Ctx.IsGrounded && !Ctx.IsSloped && Ctx.IsJumpTime == 0)
        {
            SwitchState(Factory.Grounded());
        }
        else if (Ctx.IsSloped && Ctx.IsJumpTime == 0)
        {
            SwitchState(Factory.Sloped());
        }
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction && Ctx.IsJumpTime == 0)
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
        Ctx.ExtraForce = 3.5f;

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
