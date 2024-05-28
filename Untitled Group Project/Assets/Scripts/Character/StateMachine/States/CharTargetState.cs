using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTargetState : CharBaseState
{
    public CharTargetState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Target";

        IsRootState = true;
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
        if (Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SetSubState(Factory.Grounded());
        }
        else if (Ctx.IsSloped)
        {
            SetSubState(Factory.Sloped());
        }
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SetSubState(Factory.Airborne());
        }
        else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped)
        {
            SetSubState(Factory.Jumping());
        }
    }

    public override void CheckSwitchStates()
    {
        // IDK YET
        // if (Ctx.IsTargetingAction)
        // {
        //     SwitchState(Factory.FreeLook());
        // }
    }
}
