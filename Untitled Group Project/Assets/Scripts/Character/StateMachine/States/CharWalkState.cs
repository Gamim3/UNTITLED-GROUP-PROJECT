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

        Debug.Log("Walk Enter");

        Ctx.DesiredMoveForce = Ctx.WalkSpeed;

        Ctx.MoveForce = Ctx.WalkSpeed;
    }

    public override void ExitState()
    {
        Ctx.IsWalkState = false;
    }

    public override void UpdateState()
    {

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        WalkMovement();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction || Ctx.IsJumpingState)
        {
            SwitchState(Factory.Idle());
        }
        // else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction) if not root
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsDashAction)
        {
            SwitchState(Factory.Dashing());
        }
    }

    public override void InitializeSubState() { }

    void WalkMovement()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.Movement * Ctx.MoveForce * 10f * Ctx.MoveMultiplier * Ctx.StrafeSpeedMultiplier, ForceMode.Force);
    }
}