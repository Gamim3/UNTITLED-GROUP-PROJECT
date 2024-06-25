using UnityEngine;

public class CharSlopedState : CharBaseState
{
    public CharSlopedState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        _stateName = "Sloped";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsSlopedState = true;

        Ctx.PlayerRigidBody.useGravity = false;

        // Ctx.JumpMent = new Vector3(0, 1, 0);
        Ctx.PlayerRigidBody.drag = Ctx.GroundDrag;
    }

    public override void ExitState()
    {
        Ctx.IsSlopedState = false;

        Ctx.PlayerRigidBody.useGravity = true;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
    }

    public override void FixedUpdateState()
    {
        Ctx.Movement = Ctx.GetSlopeMoveDirection(Ctx.CurrentMovement);

        if (Ctx.PlayerRigidBody.velocity.y > 0)
        {
            Ctx.MoveMultiplier = 2f;
        }

        if (!Ctx.GroundContact)
        {
            Ctx.PlayerRigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        CheckSwitchStates();
    }

    #endregion

    public override void InitializeSubState()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SetSubState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SetSubState(Factory.Running());
        }
        else if (Ctx.IsDashAction && Ctx.CanDash && !Ctx.IsStunned)
        {
            SetSubState(Factory.Dashing());
        }
        else if (Ctx.IsStunned)
        {
            SetSubState(Factory.Stun());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SwitchState(Factory.Grounded());
        }
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SwitchState(Factory.Airborne());
        }
        else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped)
        {
            SwitchState(Factory.Jumping());
        }
    }
}
