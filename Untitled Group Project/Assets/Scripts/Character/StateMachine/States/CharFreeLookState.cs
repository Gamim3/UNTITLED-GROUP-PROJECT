using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFreeLookState : CharBaseState
{
    public CharFreeLookState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "FreeLook";
    }

    public override void EnterState()
    {
        // Debug.Log("Enter FreeLook");

        InitializeSubState();

        Ctx.IsFreeLookState = true;

        Ctx.FreeLookCam.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        Ctx.IsFreeLookState = false;

        Ctx.FreeLookCam.gameObject.SetActive(false);
    }

    public override void UpdateState()
    {
        // Debug.Log("Update FreeLook");

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
        if (Ctx.IsTargetingAction)
        {
            SwitchState(Factory.Target());
        }
    }
}
