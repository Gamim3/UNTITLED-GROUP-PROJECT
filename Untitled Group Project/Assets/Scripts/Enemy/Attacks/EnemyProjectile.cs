using System;
using System.Collections;
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Entity>().TakeDamage(projectileDamage);
        }

        Destroy(gameObject);
    }

    public void AimAtPlayer()
    {
        GameObject player = GameObject.Find("PlayerObj");
        float throwAngle = GameObject.Find("Enemy").GetComponent<Enemy>().throwAngle;

        float viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -throwAngle / 2);
        float viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, throwAngle / 2);

        transform.LookAt(player.transform);

        transform.eulerAngles = new Vector3
        (
             transform.eulerAngles.x,
             Mathf.Clamp(transform.eulerAngles.y, viewAngle01, viewAngle02),
             transform.eulerAngles.z
        );
    }

    private float DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return angleInDegrees;
    }

    public IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
