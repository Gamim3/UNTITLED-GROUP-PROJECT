using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttackTest : MonoBehaviour
{
    [SerializeField] GameObject _damageNumberCanvas;
    [SerializeField] float _minDamage = 5;
    [SerializeField] float _MaxDamage = 12;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            float damageToDo = Random.Range(_minDamage, _MaxDamage);
            other.GetComponent<Entity>().TakeDamage(damageToDo);
            if (damageToDo > 10f)
            {
                StartCoroutine(HitPause());
            }
            Transform damageCanvas = Instantiate(_damageNumberCanvas, other.transform.position, Quaternion.identity).transform;
            damageCanvas.LookAt(Camera.main.transform.position);
            damageCanvas.GetComponentInChildren<TMP_Text>().text = damageToDo.ToString();
            Destroy(damageCanvas.gameObject, 1f);
        }
    }

    IEnumerator HitPause()
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
    }
}
