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

        Debug.Log("ENTER DASH");

        Ctx.IsDashingState = true;

        Ctx.IsDashTime = Ctx.MaxDashTime;

        Ctx.ResetDash = true;

        Ctx.DashMent = (Ctx.Orientation.forward * Ctx.CurrentMovementInput.y) + (Ctx.Orientation.right * Ctx.CurrentMovementInput.x);

        if (Ctx.DashMent == Vector3.zero)
        {
            Ctx.DashMent = Ctx.PlayerObj.forward;
        }

        if (Ctx.CanDash)
        {
            HandleDash();
        }
    }

    public override void ExitState()
    {
        Debug.Log("EXIT DASH");

        Ctx.CanDash = false;

        Ctx.IsDashTime = Ctx.MaxDashTime;

        Ctx.IsDashingState = false;

        Ctx.ResetDash = true;
    }

    public override void UpdateState()
    {
        Debug.Log(" Dash Update");

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        HandleDashTime();
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsStunned && Ctx.IsDashTime == 0)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsStunned && Ctx.IsDashTime == 0)
        {
            SwitchState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsStunned && Ctx.IsDashTime == 0)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsStunned && Ctx.IsDashTime == 0)
        {
            SwitchState(Factory.Stun());
        }
    }

    void HandleDash()
    {
        Ctx.IsForced = true;
        Ctx.ExtraForce = Ctx.DashForce;

        Ctx.PlayerRigidBody.AddForce(Ctx.DashMent * Ctx.DashForce, ForceMode.Impulse);
    }

    void HandleDashTime()
    {
        if (Ctx.IsJumpTime > 0)
        {
            Ctx.IsDashTime -= Time.deltaTime;
        }
        else
        {
            Ctx.IsDashTime = 0;
        }
    }
}