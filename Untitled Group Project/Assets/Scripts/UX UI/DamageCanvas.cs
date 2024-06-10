using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    [SerializeField] TMP_Text _damageTxt;
    public TMP_Text DamageTxt { get { return _damageTxt; } set { _damageTxt = value; } }

    [SerializeField] Transform _lookatPos;

    private void Start()
    {
        if (_lookatPos == null)
            _lookatPos = Camera.main.transform;
    }
    void Update()
    {
        transform.LookAt(_lookatPos);
    }
}
