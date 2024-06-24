using System;
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
        if (Ctx.IsAttack1Action)
        {
            Ctx.PlayerAnimator.SetBool("NormalAttack", true);
        }
        else
        {
            Ctx.PlayerAnimator.SetBool("NormalAttack", false);
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

    string previousAttack;

    public override void OnTriggerEnterState(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Ctx.GetCurrentAttackAnimation() == previousAttack)
            {
                previousAttack = Ctx.GetCurrentAttackAnimation();
                Ctx.HitBoxCollider.enabled = false;
                return;
            }
            else
            {
                previousAttack = Ctx.GetCurrentAttackAnimation();
                switch (previousAttack)
                {
                    case "NBAttacks1":
                        Ctx.Damage = 5;
                        break;
                    case "NBAttacks2":
                        Ctx.Damage = 7;
                        break;
                    case "NBAttacks3":
                        Ctx.Damage = 4;
                        break;
                    case "NBAttacks4":
                        Ctx.Damage = 12;
                        break;
                }
                other.GetComponentInParent<Entity>().TakeDamage(Ctx.Damage);
            }
        }
    }
}
