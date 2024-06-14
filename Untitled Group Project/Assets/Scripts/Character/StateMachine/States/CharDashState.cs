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

        Ctx.IsDashingState = true;

        Ctx.DashMent = new Vector3(Ctx.CurrentMovementInput.x, 0, Ctx.CurrentMovementInput.y);

        if (Ctx.DashMent == Vector3.zero)
        {
            Ctx.DashMent = Ctx.PlayerObj.forward;
        }

        Ctx.CanDash = false;

        HandleDash();
        Ctx.StartCoroutine(Ctx.DashCooldown());
    }

    public override void ExitState()
    {
        Ctx.IsDashingState = false;
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
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction)
        {
            SwitchState(Factory.Running());
        }
    }

    // void HandleDash(float x, float y)
    void HandleDash()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.DashMent * Ctx.DashForce, ForceMode.Impulse);
    }
}