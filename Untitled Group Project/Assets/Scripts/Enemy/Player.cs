using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] FuzzyLogic logic;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        logic.playerHealth = _healthPoints / _maxHealth * 100;
    }
}
