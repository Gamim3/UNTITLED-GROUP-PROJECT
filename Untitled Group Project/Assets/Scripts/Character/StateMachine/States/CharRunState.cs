using UnityEngine;

public class CharRunState : CharBaseState
{
    public CharRunState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        // IsRootState = true; // HOW TO MAKE WORK?
    }


    public override void EnterState()
    {
        Debug.Log(" ENTER RUN ");



        Ctx.DesiredMoveForce = Ctx.RunSpeed;

        if (Ctx.MoveForce < Ctx.RunSpeed)
        {
            Ctx.MoveForce = Ctx.RunSpeed;
        }

        InitializeSubState();
    }

    public override void ExitState()
    {

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
        if (!Ctx.IsMoveAction)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction)
        {
            SwitchState(Factory.Walking());
        }
    }

    public override void InitializeSubState()
    {

    }

    void RunMovement()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.Movement * Ctx.MoveForce * 10f * Ctx.MoveMultiplier * Ctx.StrafeSpeedMultiplier, ForceMode.Force);
    }
}
