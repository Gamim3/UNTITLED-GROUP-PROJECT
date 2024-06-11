using System;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    public float damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
