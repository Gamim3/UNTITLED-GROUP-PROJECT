using System;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public float damage = 10;
    public Enemy enemy;
    public string bodyPart;
    public GameObject damageParticles;
    public Transform particlePos;

    private bool canDealDamage;

    private bool invincible;

    private float invincibilityTimer;

    private void Start()
    {
        canDealDamage = true;
    }

    private void Update()
    {
        if (invincible)
        {
            invincibilityTimer += Time.deltaTime;
        }

        if(invincibilityTimer > 0.2f)
        {
            invincible = false;
            canDealDamage = true;
        }
        else
        {
            canDealDamage = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && canDealDamage)
        {
            if(bodyPart == "LeftClaw" && enemy.isLeftClawAtacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                GameObject kaas = Instantiate(damageParticles, particlePos.position, particlePos.rotation);
                Destroy(kaas, 4);
            }
            else if(bodyPart == "RightClaw" && enemy.isRightClawAttacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                GameObject kaas = Instantiate(damageParticles, particlePos.position, particlePos.rotation);
                Destroy(kaas, 4);
            }
            else if (bodyPart == "Head" && enemy.isHeadAtacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
                GameObject kaas = Instantiate(damageParticles, particlePos.position, particlePos.rotation);
                Destroy(kaas, 4);
            }
        }

        if (!invincible)
        {
            invincible = true;
        }
    }
}
