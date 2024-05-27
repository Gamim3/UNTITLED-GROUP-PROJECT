using UnityEngine;

public class CharIdleState : CharBaseState
{
    public CharIdleState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory) { }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState() { }


    #region MonoBehaveiours

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState() { }

    #endregion

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsMoveAction)
        {
            SwitchState(Factory.Walking());
        }
        if (Ctx.IsMoveAction && Ctx.IsRunAction)
        {
            SwitchState(Factory.Running());
        }
    }
}