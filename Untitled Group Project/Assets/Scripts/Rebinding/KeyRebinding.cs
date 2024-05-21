using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyRebinding : MonoBehaviour
{
    private static PlayerInput _playerInput;
    public static NewControls inputActions;

    public static event Action<InputAction, int> rebindStarted;
    public static event Action rebindCanceled;
    public static event Action rebindComplete;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = FindObjectOfType<PlayerInput>();
        }
    }

    private void Awake()
    {
        if (_playerInput == null)
        {
            _playerInput = FindObjectOfType<PlayerInput>();
        }
        if (inputActions == null)
        {
            inputActions = new();
        }
    }

    public static void StartRebind(InputActionReference actionReference, bool excludeMouse, int actionIndex)
    {
        // Debug.Log("STARTED REBIND");
        // Debug.Log(actionReference.action.name + " " + actionIndex);
        InputAction action = _playerInput.actions.FindAction(actionReference.action.name);

        if (action == null)
        {
            return;
        }
        if (action.bindings.Count > actionIndex && action.bindings[actionIndex].isComposite)
        {
            Debug.Log("IsComposite");
            var firstPartIndex = actionIndex + 1;

            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
            {
                Debug.Log("DoExtraRebind");
                DoRebind(action, excludeMouse, firstPartIndex, true);
            }
        }
        else if (action.bindings.Count > actionIndex && action.bindings[actionIndex].isPartOfComposite)
        {
            DoRebind(action, excludeMouse, actionIndex, false);
        }
        else
        {
            DoRebind(action, excludeMouse, actionIndex, false);
        }
    }

    private static void DoRebind(InputAction actionToRebind, bool excludeMouse, int actionIndex, bool allCompositeParts)
    {
        if (actionToRebind == null || actionIndex < 0)
        {
            return;
        }

        // keybind._actionTxt.text = $"Press a Button";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(actionIndex);

        rebind.WithBindingGroup("Gamepad");

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = actionIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
                {
                    DoRebind(actionToRebind, excludeMouse, nextBindingIndex, allCompositeParts);
                }
            }

            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");

        if (excludeMouse)
        {
            rebind.WithControlsExcluding("Mouse");
        }

        rebindStarted?.Invoke(actionToRebind, actionIndex);
        rebind.Start();
    }

    public static string GetBindingName(InputActionReference actionReference, int actionIndex)
    {
        if (_playerInput == null)
        {
            _playerInput = FindObjectOfType<PlayerInput>();
        }

        InputAction action = _playerInput.actions.FindAction(actionReference.action.name);

        if (action.bindings[actionIndex].isComposite)
        {
            // Debug.Log("IsComposite: " + keybind._actionName);
            return action.GetBindingDisplayString(0);
        }
        else if (action.bindings[actionIndex].isPartOfComposite)
        {
            // Debug.Log("IsPartOfComposite: " + actionIndex);
            return GetPartOfCompositeButton(actionIndex, action);
        }
        // Debug.Log(keybind._actionName + " " + keybind._actionIndex);
        return action.GetBindingDisplayString(actionIndex);
    }

    public static string GetPartOfCompositeButton(int compositePartIndex, InputAction action)
    {
        char[] unModifiedBindingChars = action.GetBindingDisplayString(0).ToCharArray();

        int index = 1;

        string modifiedBindingString = "";

        foreach (char c in unModifiedBindingChars)
        {
            if (c != '/')
            {
                if (index == compositePartIndex)
                {
                    modifiedBindingString += c;
                }
                else if (index > compositePartIndex)
                {
                    break;
                }
            }
            else
            {
                index++;
            }
        }

        return modifiedBindingString;
    }

    public static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (inputActions == null)
        {
            inputActions = new NewControls();
        }

        InputAction action = _playerInput.actions.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
            }
        }
    }

    public static void ResetBinding(InputActionReference actionReference, int actionIndex)
    {
        InputAction action = _playerInput.actions.FindAction(actionReference.action.name);

        if (action == null || action.bindings.Count <= actionIndex)
        {
            return;
        }

        if (action.bindings[actionIndex].isComposite)
        {
            for (int i = actionIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
            }
        }
        else
        {
            action.RemoveBindingOverride(actionIndex);
        }

        SaveBindingOverride(action);
    }
}