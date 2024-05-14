using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneRebinding : MonoBehaviour
{
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
        Debug.Log($"Rebind has been completed");
    }
}
