using UnityEngine;

public class CharRunState : CharBaseState
{
    public CharRunState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Run";
    }


    public override void EnterState()
    {
        InitializeSubState();

        Debug.Log("Enter Run");

        Ctx.IsRunState = true;

        Ctx.DesiredMoveForce = Ctx.RunSpeed;

        if (Ctx.MoveForce < Ctx.RunSpeed)
        {
            Ctx.MoveForce = Ctx.RunSpeed;
        }

    }

    public override void ExitState()
    {
        Ctx.IsRunState = false;
    }

    public override void UpdateState()
    {
        Ctx.PlayerAnimator.SetFloat("MovementX", Ctx.CurrentMovementInput.x * 2);
        Ctx.PlayerAnimator.SetFloat("MovementY", Ctx.CurrentMovementInput.y * 2);

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        RunMovement();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsDashAction)
        {
            SwitchState(Factory.Dashing());
        }
    }

    public override void InitializeSubState() { }

    void RunMovement()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.Movement * Ctx.MoveForce * 10f * Ctx.MoveMultiplier * Ctx.StrafeSpeedMultiplier, ForceMode.Force);
    }
}
