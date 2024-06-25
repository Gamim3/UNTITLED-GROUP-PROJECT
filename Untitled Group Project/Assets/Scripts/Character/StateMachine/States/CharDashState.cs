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

        Ctx.PlayerAnimator.SetTrigger("Dash");

        Ctx.IsDashingState = true;

        Ctx.DashMent = (Ctx.Orientation.forward * Ctx.CurrentMovementInput.y) + (Ctx.Orientation.right * Ctx.CurrentMovementInput.x);

        if (Ctx.DashMent == Vector3.zero)
        {
            Ctx.DashMent = Ctx.PlayerObj.forward;
        }

        Ctx.CanDash = false;

        HandleDash();
    }

    public override void ExitState()
    {
        Ctx.IsDashingState = false;

        Ctx.StartCoroutine(Ctx.DashCooldown());
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsStunned)
        {
            SwitchState(Factory.Stun());
        }
    }

    // void HandleDash(float x, float y)
    void HandleDash()
    {
        Ctx.IsForced = true;
        Ctx.ExtraForce = Ctx.DashForce;

        Ctx.PlayerRigidBody.AddForce(Ctx.DashMent * Ctx.DashForce, ForceMode.Impulse);
    }
}