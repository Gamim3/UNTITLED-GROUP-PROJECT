using UnityEngine;

public class CharWalkState : CharBaseState
{
    public CharWalkState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        // IsRootState = true; // HOW TO MAKE WORK?
    }

    public override void EnterState()
    {
        InitializeSubState();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {
        WalkMovement();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMoveAction)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }

    void WalkMovement()
    {
        Ctx.PlayerRigidBody.AddForce(Ctx.Movement * Ctx.MoveForce * 10f * Ctx.MoveMultiplier * Ctx.StrafeSpeedMultiplier, ForceMode.Force);
    }
}