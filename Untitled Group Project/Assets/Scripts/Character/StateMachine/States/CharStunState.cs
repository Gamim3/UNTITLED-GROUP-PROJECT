using UnityEngine;

public class CharStunState : CharBaseState
{
    public CharStunState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Stun";
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsStunnedState = true;

        Ctx.StunTime = Ctx.MaxStunTime;
    }

    public override void ExitState()
    {
        Ctx.IsStunnedState = false;

        Ctx.IsStunned = false;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();

        HandleStunTime();
    }

    public override void FixedUpdateState() { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Walking());
        }
        // else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction) if not root
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction && !Ctx.IsStunned)
        {
            SwitchState(Factory.Running());
        }
        else if (Ctx.IsDashAction && Ctx.CanDash && !Ctx.IsStunned)
        {
            SwitchState(Factory.Dashing());
        }
    }

    public override void InitializeSubState() { }

    public void HandleStunTime()
    {
        if (Ctx.StunTime > 0)
        {
            Ctx.StunTime -= Time.deltaTime;
        }
        else
        {
            Ctx.StunTime = 0;
            Ctx.IsStunned = false;
        }
    }
}