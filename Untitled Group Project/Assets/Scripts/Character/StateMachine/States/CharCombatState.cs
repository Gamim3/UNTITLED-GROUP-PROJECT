using UnityEngine;

public class CharCombatState : CharBaseState
{
    public CharCombatState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();

        Ctx.IsCombatState = true;
    }

    public override void ExitState()
    {
        Ctx.IsCombatState = false;
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
