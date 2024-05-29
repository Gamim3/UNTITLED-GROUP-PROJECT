using UnityEngine;

public class CharCombatState : CharBaseState
{
    public CharCombatState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Combat";

        IsRootState = true;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Combat");

        InitializeSubState();

        Ctx.IsCombatState = true;
    }

    public override void ExitState()
    {
        Ctx.IsCombatState = false;
    }

    public override void UpdateState()
    {
        Debug.Log("Update Combat");

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsTargetingAction)
        {
            SetSubState(Factory.FreeLook());
        }
        else if (Ctx.IsTargetingAction)
        {
            SetSubState(Factory.Target());
        }
    }

    public override void CheckSwitchStates()
    {

    }
}
