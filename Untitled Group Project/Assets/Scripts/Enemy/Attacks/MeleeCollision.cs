using System;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public float damage = 10;
    public Enemy enemy;
    public string bodyPart;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(bodyPart == "LeftClaw" && enemy.isLeftClawAtacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
            }
            else if(bodyPart == "RightClaw" && enemy.isRightClawAttacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
            }
            else if (bodyPart == "Head" && enemy.isHeadAtacking)
            {
                collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
            }
        }
    }
}
