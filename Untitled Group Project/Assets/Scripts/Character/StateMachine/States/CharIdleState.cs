using UnityEngine;

public class CharIdleState : CharBaseState
{
    public CharIdleState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory) { }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsIdleState = true;
    }

    public override void ExitState()
    {
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
        if (Ctx.IsMoveAction && Ctx.IsRunAction)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsMoveAction)
        {
            SwitchState(Factory.Walking());
        }
    }
}