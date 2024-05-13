using UnityEngine;

public class CharAirborneState : CharBaseState
{
    public CharAirborneState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        Ctx.IsAired = true;
        Ctx.MoveMultiplier = Ctx.AirSpeed;
        Ctx.ForceSlowDownRate = 1;
    }

    public override void ExitState()
    {
        Ctx.IsAired = false;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
        Ctx.Movement = Ctx.CurrentMovement.normalized;
    }

    public override void FixedUpdateState()
    {
        CheckSwitchStates();
    }

    #endregion

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchStates()
    {

    }
}