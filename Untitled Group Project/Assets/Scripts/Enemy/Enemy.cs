using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] Transform playerPosition;
    [SerializeField] float distance;

    [Header ("Dependancy")]
    [SerializeField] EnemyBrain brain;
    [SerializeField] FuzzyLogic logic;

    public override void Update()
    {
        base.Update();

        distance = Vector3.Distance(transform.position, playerPosition.position);

        logic.enemyHealth = healthPoints / maxHealth * 100;
        logic.energy = energy / maxEnergy * 100;
        logic.distance = distance;
    }
}
