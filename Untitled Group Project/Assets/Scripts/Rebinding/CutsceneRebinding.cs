using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneRebinding : MonoBehaviour
{

    [SerializeField] bool _completedRebind;

    [SerializeField] bool _isRebinding;

    [SerializeField] int _actionToRebind;

    [SerializeField] List<Keybind> _cutsceneActions;

    [Serializable]
    public struct Keybind
    {
        public InputActionReference _inputActionReference;
        public int _actionIndex;
        public bool _excludeMouse;

        public Keybind(InputActionReference inputActionReference, int actionIndex, bool excludeMouse)
        {
            _inputActionReference = inputActionReference;
            _actionIndex = actionIndex;
            _excludeMouse = excludeMouse;
        }
    }

    private void OnEnable()
    {
        KeyRebinding.rebindComplete += CompleteRebind;
    }

    private void OnDisable()
    {
        KeyRebinding.rebindComplete -= CompleteRebind;
    }

    void CompleteRebind()
    {
        Debug.Log($"Rebind has been completed with binding: ( {KeyRebinding.GetBindingName(_cutsceneActions[_actionToRebind]._inputActionReference, _cutsceneActions[_actionToRebind]._actionIndex)} )");




        _completedRebind = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_completedRebind)
        {
            _completedRebind = false;

            Debug.Log($"Rebind has been saved");

            _actionToRebind++;

            StartNewRebind(_actionToRebind);
        }
    }

    private void Start()
    {
        StartNewRebind(0);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            StartNewRebind(_actionToRebind);
            Debug.Log("YES");
        }
        else
        {
            _isRebinding = false;
            _completedRebind = false;
            Debug.Log("NO");
        }

    }

    void StartNewRebind(int index)
    {
        if (!_isRebinding)
        {
            _isRebinding = true;

            if (!_completedRebind)
            {
                KeyRebinding.StartRebind(_cutsceneActions[index]._inputActionReference, _cutsceneActions[index]._excludeMouse, _cutsceneActions[index]._actionIndex);
            }
        }

    }

}
