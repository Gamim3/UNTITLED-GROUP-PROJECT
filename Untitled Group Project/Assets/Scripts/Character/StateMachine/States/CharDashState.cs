using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDashState : CharBaseState
{
    public CharDashState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Dash";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsIdleState = true;

        HandleDash();
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
        if (!Ctx.IsMoveAction)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SwitchState(Factory.Running());
        }
    }

    // void HandleDash(float x, float y)
    void HandleDash()
    {
        Ctx.PlayerRigidBody.AddForce(new Vector3(0, 0, 1) * Ctx.DashForce, ForceMode.Impulse);
    }

}