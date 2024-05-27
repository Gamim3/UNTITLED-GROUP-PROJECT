using System;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent.gameObject.GetComponent<Entity>().TakeDamage(10);
        }
    }
}
