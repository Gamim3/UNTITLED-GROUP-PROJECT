using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.InputSystem.LowLevel;

public class RebindingUI : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    [SerializeField] string _inputTest;

    [SerializeField] List<Keybind> _keyboardKeybinds = new List<Keybind>();
    [SerializeField] List<Keybind> _controllerKeybinds = new List<Keybind>();
    [SerializeField] List<Keybind> _combinedKeybinds = new List<Keybind>();

    [Serializable]
    public struct Keybind
    {
        public InputActionReference _inputActionReference;
        public int _actionIndex;
        // public string _actionName;
        public bool _excludeMouse;
        public TMP_Text _actionTxt;

        public Keybind(InputActionReference inputActionReference, int actionIndex, string actionName, bool excludeMouse, TMP_Text actionTxt)
        {
            _inputActionReference = inputActionReference;
            _actionIndex = actionIndex;
            // _actionName = inputActionReference.action.name;
            _excludeMouse = excludeMouse;
            _actionTxt = actionTxt;
        }
    }

    private void OnEnable()
    {
        if (_playerInput != null)
        {
            for (int i = 0; i < _keyboardKeybinds.Count; i++)
            {
                KeyRebinding.LoadBindingOverride(_keyboardKeybinds[i]._inputActionReference.action.name);
            }
            UpdateAllUI();
        }

        KeyRebinding.rebindComplete += UpdateAllUI;
        KeyRebinding.rebindCanceled += UpdateAllUI;

        string actionName = _keyboardKeybinds[0]._inputActionReference.action.name;
        Debug.Log($"Check the action name: {actionName}");
    }

    private void OnDisable()
    {
        KeyRebinding.rebindComplete -= UpdateAllUI;
        KeyRebinding.rebindCanceled -= UpdateAllUI;
    }

    private void OnValidate()
    {
        if (_playerInput == null)
        {
            return;
        }

        UpdateAllUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            KeyboardRebindBtn(0);
        }
    }

    private void UpdateAllUI()
    {
        for (int i = 0; i < _keyboardKeybinds.Count; i++)
        {
            if (_keyboardKeybinds[i]._actionTxt != null)
            {
                if (Application.isPlaying)
                {
                    _keyboardKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_keyboardKeybinds[i]);
                    _inputTest = KeyRebinding.GetBindingName(_keyboardKeybinds[i]);
                }
                else
                {
                    _keyboardKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_keyboardKeybinds[i]);
                    _inputTest = KeyRebinding.GetBindingName(_keyboardKeybinds[i]);
                }

            }
        }
        for (int i = 0; i < _controllerKeybinds.Count; i++)
        {
            if (_controllerKeybinds[i]._actionTxt != null)
            {
                if (Application.isPlaying)
                {
                    _controllerKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_controllerKeybinds[i]);
                }
                else
                {
                    _controllerKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_controllerKeybinds[i]);
                }

            }
        }
        for (int i = 0; i < _combinedKeybinds.Count; i++)
        {
            if (_combinedKeybinds[i]._actionTxt != null)
            {
                if (Application.isPlaying)
                {
                    _combinedKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_combinedKeybinds[i]);
                }
                else
                {
                    _combinedKeybinds[i]._actionTxt.text = KeyRebinding.GetBindingName(_combinedKeybinds[i]);
                }

            }
        }
    }

    private void SaveAllBindings()
    {
        SaveKeyboardBindings();
        SaveControllerBindings();
        SaveCombinedBindings();
    }

    #region Keyboard

    public void KeyboardRebindBtn(int keybindIndex)
    {
        KeyRebinding.StartRebind(_keyboardKeybinds[keybindIndex]);
    }

    public void ResetKeyboardBindingBtn(int keybindIndex)
    {
        KeyRebinding.ResetBinding(_keyboardKeybinds[keybindIndex]);
        UpdateAllUI();
    }

    public void ReloadKeyboardBindingBtn(int keybindIndex)
    {
        KeyRebinding.LoadBindingOverride(_keyboardKeybinds[keybindIndex]._inputActionReference.action.name);
        UpdateAllUI();
    }

    public void ResetKeyboardBindings()
    {
        for (int i = 0; i < _keyboardKeybinds.Count; i++)
        {
            KeyRebinding.ResetBinding(_keyboardKeybinds[i]);
        }
        UpdateAllUI();
    }

    public void ReloadKeyboardBindings()
    {
        for (int i = 0; i < _keyboardKeybinds.Count; i++)
        {
            KeyRebinding.LoadBindingOverride(_keyboardKeybinds[i]._inputActionReference.action.name);
        }
        UpdateAllUI();
    }

    public void SaveKeyboardBindings()
    {
        for (int i = 0; i < _keyboardKeybinds.Count; i++)
        {
            KeyRebinding.SaveBindingOverride(_keyboardKeybinds[i]._inputActionReference);
        }
    }

    #endregion

    #region Controller

    public void ControllerRebindBtn(int keybindIndex)
    {
        KeyRebinding.StartRebind(_controllerKeybinds[keybindIndex]);
    }

    public void ResetControllerBindings()
    {
        for (int i = 0; i < _controllerKeybinds.Count; i++)
        {
            KeyRebinding.ResetBinding(_controllerKeybinds[i]);
        }
        UpdateAllUI();
    }

    public void ReloadControllerBindings()
    {
        for (int i = 0; i < _controllerKeybinds.Count; i++)
        {
            KeyRebinding.LoadBindingOverride(_controllerKeybinds[i]._inputActionReference.action.name);
        }
        UpdateAllUI();
    }

    public void SaveControllerBindings()
    {
        for (int i = 0; i < _controllerKeybinds.Count; i++)
        {
            KeyRebinding.SaveBindingOverride(_controllerKeybinds[i]._inputActionReference);
        }
    }

    #endregion

    #region Combined

    public void CombinedRebindBtn(int keybindIndex)
    {
        KeyRebinding.StartRebind(_combinedKeybinds[keybindIndex]);
    }

    public void ResetCombinedBindings()
    {
        for (int i = 0; i < _combinedKeybinds.Count; i++)
        {
            KeyRebinding.ResetBinding(_combinedKeybinds[i]);
        }
        UpdateAllUI();
    }

    public void ReloadCombinedBindings()
    {
        for (int i = 0; i < _combinedKeybinds.Count; i++)
        {
            KeyRebinding.LoadBindingOverride(_combinedKeybinds[i]._inputActionReference.action.name);
        }
        UpdateAllUI();
    }

    public void SaveCombinedBindings()
    {
        for (int i = 0; i < _combinedKeybinds.Count; i++)
        {
            KeyRebinding.SaveBindingOverride(_combinedKeybinds[i]._inputActionReference);
        }
    }

    #endregion
}
