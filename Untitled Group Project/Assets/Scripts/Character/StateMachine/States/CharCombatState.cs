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
        // Debug.Log("Update Combat");

        // Ctx.PlayerAnimator.GetCurrentAnimatorClipInfo(0).

        // CHECK IF STILL ATTACK ANIMATION
        if (Ctx.IsAttack1Action)
        {
            Ctx.PlayerAnimator.SetInteger("NormalAttack", 4);
        }
        else
        {
            Ctx.PlayerAnimator.SetInteger("NormalAttack", 0);
        }

        if (Ctx.CheckAttackAnimation())
        {
            // Debug.Log($"Hitbox Active, animation playing: {current_animation}");
            Ctx.Damage = 3;


            // if (current_animation == "NBAttacks1")
            // {
            //     Debug.Log("Attack 1");
            // }
            // if (current_animation == "NBAttacks2")
            // {
            //     Ctx.HitBoxCollider.GetComponent<AttackTest>().damage = 2;
            //     Debug.Log("Attack 2");
            // }
            // if (current_animation == "NBAttacks3")
            // {
            //     Ctx.HitBoxCollider.GetComponent<AttackTest>().damage = 3;
            //     Debug.Log("Attack 3");
            // }
            // if (current_animation == "NBAttacks4")
            // {
            //     Ctx.HitBoxCollider.GetComponent<AttackTest>().damage = 15;
            //     Debug.Log("Attack 4");
            // }

            Ctx.HitBoxCollider.enabled = true;
        }
        else
        {
            Debug.Log("Hitbox Inactive");
            Ctx.HitBoxCollider.enabled = false;
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

    public override void OnTriggerEnterState(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Ctx.GetCurrentAttackAnimation();

            Ctx.HitBoxCollider.enabled = false;
            // float damageToDo = Random.Range(_minDamage, _MaxDamage);
            other.GetComponent<Entity>().TakeDamage(Ctx.Damage);

            // Vector3 newPosition = other.transform.position + _spawnOffset;

            // Transform damageCanvas = Instantiate(Ctx.DamageCanvas, newPosition, Quaternion.identity).transform;
            // damageCanvas.LookAt(Camera.main.transform.position); 
            // damageCanvas.GetComponentInChildren<TMP_Text>().text = damage.ToString();
            // Destroy(damageCanvas.gameObject, 3f);
        }
    }
}
