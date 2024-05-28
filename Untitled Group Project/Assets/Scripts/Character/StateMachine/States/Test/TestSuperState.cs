using UnityEngine;

public class TestSuperState : CharBaseState
{
    public TestSuperState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsSuperState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Super State Enter");
        InitializeSubState();
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        Debug.Log("Super State Update");
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
        // SetSubState(Factory.Sub());
    }
}
