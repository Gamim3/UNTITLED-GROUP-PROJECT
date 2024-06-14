using UnityEngine;

public class CharIdleState : CharBaseState
{
    public CharIdleState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Idle";
    }

    public override void EnterState()
    {
        // Debug.Log("Enter Idle");

        InitializeSubState();

        Ctx.IsIdleState = true;
    }

    public override void ExitState()
    {
        // Debug.Log("Exit Idle");

        Ctx.IsIdleState = false;
    }



    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && Ctx.IsJumpTime == 0 && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && Ctx.IsJumpTime == 0 && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsDashAction && Ctx.CanDash)
        {
            SwitchState(Factory.Dashing());
        }
    }
}