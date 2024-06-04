using UnityEngine;

public class CharWalkState : CharBaseState
{
    public CharWalkState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Walk";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsWalkState = true;

        // Debug.Log("Walk Enter");

        Ctx.DesiredMoveForce = Ctx.WalkSpeed;

        Ctx.MoveForce = Ctx.WalkSpeed;
    }

    public override void ExitState()
    {
        Ctx.IsWalkState = false;
    }

    public override void UpdateState()
    {
        Ctx.PlayerAnimator.SetFloat("MovementX", Ctx.CurrentMovementInput.x);
        Ctx.PlayerAnimator.SetFloat("MovementY", Ctx.CurrentMovementInput.y);

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        WalkMovement();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction || Ctx.IsJumpingState)
        {
            SwitchState(Factory.Idle());
        }
        // else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction) if not root
        else if (Ctx.IsMoveAction && Ctx.IsRunAction)
        {
            SwitchState(Factory.Running());
        }
        // IDK IF DASH IS ROOT STATE OR NOT
        // else if (Ctx.IsDashAction)
        // {
        //     SetSubState(Factory.Dashing());
        // }
    }

    public override void InitializeSubState() { }

    void WalkMovement()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.Movement * Ctx.MoveForce * 10f * Ctx.MoveMultiplier * Ctx.StrafeSpeedMultiplier, ForceMode.Force);
    }
}