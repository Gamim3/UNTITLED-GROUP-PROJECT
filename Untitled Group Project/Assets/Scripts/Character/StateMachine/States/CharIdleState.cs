using UnityEngine;

public class CharIdleState : CharBaseState
{
    public CharIdleState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Idle";
    }

    public override void EnterState()
    {
        Debug.Log("Enter Idle");

        InitializeSubState();

        Ctx.IsIdleState = true;
    }

    public override void ExitState()
    {
        Debug.Log("Exit Idle");

        Ctx.IsIdleState = false;
    }



    public override void UpdateState()
    {
        Debug.Log("Update Idle");

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        Debug.Log("Fixed Idle");
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Running());
        }
        // IDK IF DASH IS ROOT STATE OR NOT
        // else if (Ctx.IsDashAction)
        // {
        //     SetSubState(Factory.Dashing());
        // }
    }
}