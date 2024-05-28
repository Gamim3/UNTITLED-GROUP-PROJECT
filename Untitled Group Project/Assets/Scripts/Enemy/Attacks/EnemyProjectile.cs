using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [NonSerialized] public float projectileSpeed;
    [NonSerialized] public float projectileDamage;

    void Start()
    {
        StartCoroutine(DespawnTimer());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
             collision.gameObject.GetComponent<Entity>().TakeDamage(projectileDamage);
        }

        Destroy(gameObject);
    }

    public IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
