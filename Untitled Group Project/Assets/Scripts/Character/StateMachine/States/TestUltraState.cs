using UnityEngine;

public class TestUltraState : CharBaseState
{
    public TestUltraState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Ultra State Enter");
        InitializeSubState();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("Ultra State Update");
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void LateUpdateState()
    {

    }

    public override void CheckSwitchStates()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Super());
    }
}
