using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [SerializeField] GameObject _damageNumberCanvas;
    [SerializeField] int _minDamage = 5;
    [SerializeField] int _MaxDamage = 12;

    [SerializeField] Vector3 _spawnOffset = new Vector3(0, 2, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.LogWarning("Attacking");
            float damageToDo = Random.Range(_minDamage, _MaxDamage);
            other.GetComponentInParent<Entity>().TakeDamage(damageToDo);
            if (damageToDo > 10f)
            {
                StartCoroutine(HitPause());
            }
            Transform damageCanvas = Instantiate(_damageNumberCanvas, other.transform.position + _spawnOffset, Quaternion.identity).transform;
            damageCanvas.LookAt(Camera.main.transform.position);
            damageCanvas.GetComponentInChildren<TMP_Text>().text = damageToDo.ToString();
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
