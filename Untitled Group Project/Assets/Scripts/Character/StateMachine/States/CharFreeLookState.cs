using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFreeLookState : CharBaseState
{
    public CharFreeLookState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true; // idk
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsFreeLookState = true;
    }

    public override void ExitState()
    {
        Ctx.IsFreeLookState = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {

    }
}
