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

        Ctx.PlayerAnimator.SetTrigger("Jump");

        Ctx.IsJumpTime = Ctx.MaxJumpTime;

        Ctx.IsExitingSlope = true;

        Ctx.PlayerRigidBody.drag = 0;

        HandleJump();
    }

    public override void ExitState()
    {
        Ctx.IsJumpingState = false;
        Ctx.IsForced = false;
        Ctx.IsExitingSlope = false;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
        Ctx.Movement = Vector3.zero;
    }

    public override void FixedUpdateState()
    {
        CheckSwitchStates();
        HandleJumpTime();
    }

    #endregion

    public override void InitializeSubState()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && Ctx.IsJumpTime == 0 && !Ctx.IsStunned)
        {
            SetSubState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction && Ctx.IsJumpTime == 0 && !Ctx.IsStunned)
        {
            SetSubState(Factory.Running());
        }
        else if (Ctx.IsDashAction && Ctx.CanDash && !Ctx.IsStunned)
        {
            Debug.Log("Dash from Jumping");

            SetSubState(Factory.Dashing());
        }
        else if (Ctx.IsStunned)
        {
            SetSubState(Factory.Stun());
        }
    }

    public override void CheckSwitchStates()
    {
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
    }

    void HandleJump()
    {
        Ctx.IsForced = true;
        Ctx.ExtraForce = Ctx.JumpForce;

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
