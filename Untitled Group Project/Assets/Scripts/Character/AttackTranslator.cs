using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTranslator : MonoBehaviour
{
    [SerializeField] CharStateMachine charStateMachine;



    private void Awake()
    {
        charStateMachine = GetComponentInParent<CharStateMachine>();
    }

    public void AttackStart(float damage)
    {
        charStateMachine.HitBoxCollider.enabled = true;
        charStateMachine.Damage = damage;
    }

    public void AttackEnd()
    {
        charStateMachine.HitBoxCollider.enabled = false;
    }
}
