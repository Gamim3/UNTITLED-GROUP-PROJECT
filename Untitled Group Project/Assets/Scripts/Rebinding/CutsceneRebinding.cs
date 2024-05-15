using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.HID.HID;

public class CutsceneRebinding : MonoBehaviour
{

    private Keyboard _keyboard;
    private KeyboardState _keyboardState;

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

    private void Awake()
    {
        _keyboard = InputSystem.GetDevice<Keyboard>();
        _keyboardState = new KeyboardState();
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



        // var Gamepad = InputSystem.GetDevice<Gamepad>();
        // var GamepadState = new GamepadState();
        // InputSystem.QueueStateEvent(Gamepad, GamepadState);





        // GamepadState.WithButton((GamepadButton)(Button)System.Enum.Parse(typeof(Button), KeyRebinding.GetBindingName(_cutsceneActions[_actionToRebind]._inputActionReference, _cutsceneActions[_actionToRebind]._actionIndex)));

        // GamepadState.WithButton();
        // GamepadState.Release(Key.Space);
        // InputSystem.QueueStateEvent(Keyboard, KeyboardState);




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
        // _keyboardState.Press((Key)System.Enum.Parse(typeof(Key), KeyRebinding.GetBindingName(_cutsceneActions[_actionToRebind]._inputActionReference, _cutsceneActions[_actionToRebind]._actionIndex)));

        if (Input.anyKey && !_cutsceneActions[_actionToRebind]._inputActionReference.action.IsPressed())
        {
            _keyboardState.Release(Key.Slash);
            InputSystem.QueueStateEvent(_keyboard, _keyboardState);
            // _keyboardState.Press(Key.);
            InputSystem.QueueStateEvent(_keyboard, _keyboardState);



        }
        // {
        //     StartNewRebind(_actionToRebind);
        //     Debug.Log("YES");

        //     // InputSystem.QueueStateEvent(_keyboard, _keyboardState);

        // }
        // else
        // {
        //     _isRebinding = false;
        //     _completedRebind = false;
        //     Debug.Log("NO");
        // }

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
