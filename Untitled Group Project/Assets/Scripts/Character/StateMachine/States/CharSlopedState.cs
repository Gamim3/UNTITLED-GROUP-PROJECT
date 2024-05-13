using UnityEngine;

public class CharSlopedState : CharBaseState
{
    public CharSlopedState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        Ctx.PlayerRigidBody.useGravity = false;

        Ctx.JumpMent = new Vector3(0, 1, 0);
    }

    public override void ExitState()
    {
        Ctx.PlayerRigidBody.useGravity = true;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
        Ctx.Movement = Ctx.GetSlopeMoveDirection(Ctx.CurrentMovement);

        if (Ctx.PlayerRigidBody.velocity.y > 0)
        {
            Ctx.MoveMultiplier = 2f;
        }

        if (Ctx.PlayerRigidBody.velocity.y > 0 || Ctx.PlayerRigidBody.velocity.y > 0)
        {
            Ctx.PlayerRigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
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
