using UnityEngine;

public class CharIdleState : CharBaseState
{
    public CharIdleState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("IDLE ENTER");

        InitializeSubState();
    }

    public override void ExitState() { }


    #region MonoBehaveiours

    public override void UpdateState()
    {
        Debug.Log("IDLE UPDATE");

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
    }
}