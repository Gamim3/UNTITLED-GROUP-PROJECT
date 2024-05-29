using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Entity
{
    [SerializeField] Animator _dummyAnimator;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (_healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void Exhaustion(float Energy)
    {
        base.Exhaustion(Energy);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (Random.Range(0, 2) == 0)
        {
            _dummyAnimator.SetTrigger("Spin");
        }
        else
        {
            _dummyAnimator.SetTrigger("HeadSpin");
        }
    }

}
