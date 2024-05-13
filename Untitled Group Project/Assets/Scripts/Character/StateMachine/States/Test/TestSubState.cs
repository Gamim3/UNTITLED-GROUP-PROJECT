using UnityEngine;

public class TestSubState : CharBaseState
{
    public TestSubState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Sub State Enter");
        InitializeSubState();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("Sub State Update");
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {

    }

    public override void InitializeSubState()
    {

    }
}
