using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [SerializeField] CharStateMachine _stateMachine;
    [SerializeField] GameObject _damageNumberCanvas;
    // [SerializeField] int _minDamage = 5;
    // [SerializeField] int _MaxDamage = 12;
    public int damage;


    [SerializeField] Vector3 _spawnOffset = new Vector3(0, 2, 0);

    private void OnTriggerEnter(Collider other)
    {
        // _stateMachine.OnTriggerEnter(other);

        if (other.CompareTag("Enemy"))
        {
            Debug.LogWarning("Attacking");

            this.GetComponent<Collider>().enabled = false;
            // float damageToDo = Random.Range(_minDamage, _MaxDamage);
            other.GetComponentInParent<Entity>().TakeDamage(damage);

            if (damage > 10f)
            {
                StartCoroutine(HitPause());
            }

            Vector3 newPosition = other.transform.position + _spawnOffset;

            Transform damageCanvas = Instantiate(_damageNumberCanvas, newPosition, Quaternion.identity).transform;
            damageCanvas.LookAt(Camera.main.transform.position);
            damageCanvas.GetComponentInChildren<TMP_Text>().text = damage.ToString();
            Destroy(damageCanvas.gameObject, 3f);
        }
    }

    IEnumerator HitPause()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1;
    }
}
