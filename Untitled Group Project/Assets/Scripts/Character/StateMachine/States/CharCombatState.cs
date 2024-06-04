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
        // Debug.Log("Enter Combat");

        InitializeSubState();

        Ctx.IsCombatState = true;
    }

    public override void ExitState()
    {
        Ctx.IsCombatState = false;
    }

    public override void UpdateState()
    {
        // Debug.Log("Update Combat");

        // Ctx.PlayerAnimator.GetCurrentAnimatorClipInfo(0).

        // CHECK IF STILL ATTACK ANIMATION
        if (Ctx.IsAirborneState)
        {
            if (Ctx.IsAttack1Action)
            {
                Ctx.PlayerAnimator.SetInteger("NormalAttack", 2);
            }
            else
            {
                Ctx.PlayerAnimator.SetInteger("NormalAttack", 0);
            }
        }
        else
        {
            if (Ctx.IsAttack1Action)
            {
                Ctx.PlayerAnimator.SetInteger("NormalAttack", 4);
            }
            else
            {
                Ctx.PlayerAnimator.SetInteger("NormalAttack", 0);
            }
        }

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
