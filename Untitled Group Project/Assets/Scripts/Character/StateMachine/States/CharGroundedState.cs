using UnityEngine;
public class CharGroundedState : CharBaseState
{
    public CharGroundedState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "Grounded";
    }

    public override void EnterState()
    {
        // Debug.Log("Enter Grounded");

        InitializeSubState();

        Ctx.IsGroundedState = true;

        Ctx.MoveMultiplier = 1f;
        Ctx.ForceSlowDownRate = 5;
        Ctx.IsAirborneState = false;
        // Ctx.DesiredMoveForce = Ctx.WalkSpeed;
        Ctx.JumpMent = new Vector3(0, 1, 0);

        Ctx.JumpAmount = 1; // idk yet

        if (Ctx.MoveForce < Ctx.WalkSpeed)
        {
            Ctx.MoveForce = Ctx.WalkSpeed;
        }
    }

    public override void ExitState()
    {
        // Debug.Log("Exit Grounded");

        Ctx.IsGroundedState = false;
    }

    #region MonoBehaveiours

    public override void UpdateState()
    {
        // Debug.Log("Update Grounded");

        Ctx.Movement = Ctx.CurrentMovement.normalized;

        CheckSwitchStates();
    }

    public override void FixedUpdateState()
    {

    }

    #endregion

    public override void InitializeSubState()
    {
        if (!Ctx.IsMoveAction)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMoveAction && !Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Walking());
        }
        else if (Ctx.IsMoveAction && Ctx.IsRunAction && !Ctx.IsDashAction)
        {
            SetSubState(Factory.Running());
        }
        else if (Ctx.IsDashAction)
        {
            // DASH STATE
            // SetSubState(Factory.Dash()); 
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsSloped)
        {
            SwitchState(Factory.Sloped());
        }
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SwitchState(Factory.Airborne());
        }
        else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped)
        {
            SwitchState(Factory.Jumping());
        }
    }
}