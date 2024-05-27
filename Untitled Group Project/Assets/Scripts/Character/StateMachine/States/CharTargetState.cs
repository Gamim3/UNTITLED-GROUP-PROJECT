using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTargetState : CharBaseState
{
    public CharTargetState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true; // idk
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsTargetingState = true;
    }

    public override void ExitState()
    {
        Ctx.IsTargetingState = false;
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
