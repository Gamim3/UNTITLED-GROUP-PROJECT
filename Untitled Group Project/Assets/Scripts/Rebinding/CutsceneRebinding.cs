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

    [SerializeField] string _currentKeyboardInputIndex;

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

        _currentKeyboardInputIndex = GetKeyboardInputNameFromName(KeyRebinding.GetBindingName(_cutsceneActions[_actionToRebind]._inputActionReference, _cutsceneActions[_actionToRebind]._actionIndex));

        var Gamepad = InputSystem.GetDevice<Gamepad>();
        var GamepadState = new GamepadState();
        // GamepadState.WithButton((GamepadButton)(Button)System.Enum.Parse(typeof(Button), ));
        Gamepad.leftStick.up.IsPressed();
        InputSystem.QueueStateEvent(Gamepad, GamepadState);



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

    public bool test;

    private List<(string, string)> stringPairs = new List<(string, string)>
    {
        ("None", ""),
        ("Space", "Space"),
        ("Enter", "Enter"),
        ("Tab", "Tab"),
        ("Backquote", "Grave"),
        ("Backquote", "``"),
        ("Quote", "''"),
        ("Quote", "Acute"),
        ("Semicolon", ";"),
        ("Comma", ","),
        ("Period", "."),
        ("Slash", "/"),
        ("Backslash", "\\"),
        ("LeftBracket", "["),
        ("RightBracket", "]"),
        ("Minus", "-"),
        ("Equals", "="),
        ("A", "A"),
        ("B", "B"),
        ("C", "C"),
        ("D", "D"),
        ("E", "E"),
        ("F", "F"),
        ("G", "G"),
        ("H", "H"),
        ("I", "I"),
        ("J", "J"),
        ("K", "K"),
        ("L", "L"),
        ("M", "M"),
        ("N", "N"),
        ("O", "O"),
        ("P", "P"),
        ("Q", "Q"),
        ("R", "R"),
        ("S", "S"),
        ("T", "T"),
        ("U", "U"),
        ("V", "V"),
        ("W", "W"),
        ("X", "X"),
        ("Y", "Y"),
        ("Z", "Z"),
        ("Digit1", "1"),
        ("Digit2", "2"),
        ("Digit3", "3"),
        ("Digit4", "4"),
        ("Digit5", "5"),
        ("Digit6", "6"),
        ("Digit7", "7"),
        ("Digit8", "8"),
        ("Digit9", "9"),
        ("Digit0", "0"),
        ("LeftShift", "Shift"),
        ("RightShift", "Right Shift"),
        ("LeftAlt", "Alt"),
        ("RightAlt", "Right Alt"),
        ("AltGr", "Right Alt"),
        ("LeftCtrl", "Ctrl"),
        ("LeftWindows", "Left Windows"),
        ("RightWindows", "Right Windows"), // TEST GERLOF
        ("RightCtrl", "Right Ctrl"),
        ("LeftMeta", "Left Windows"),
        ("RightMeta", "Right Windows"), // not sure
        ("LeftApple", "Left Windows"), // not sure
        ("RightApple", "Right Windows"), // not sure
        ("LeftCommand", "Left Command"), // not sure
        ("RightCommand", "Right Command"), // not sure
        ("ContextMenu", "Application"), // BAS TEST
        ("Escape", "Escape"),
        ("LeftArrow", "Left"),
        ("RightArrow", "Right"),
        ("UpArrow", "Up"),
        ("DownArrow", "Down"),
        ("Backspace", "Backspace"),
        ("PageDown", "Pgdown"),
        ("PageUp", "Pgup"),
        ("Home", "Home"),
        ("End", "End"),
        ("Insert", "Insert"), // BAS TEST
        ("Delete", "Delete"),
        ("CapsLock", "CapsLock"),
        ("NumLock", "Num Lock"),
        ("PrintScreen", "Prnt Scrn"),
        ("ScrollLock", "Scroll Lock"),
        ("Pause", "Break"), // BAS TEST
        ("NumpadEnter", "Num Enter"),
        ("NumpadDivide", "Num Divide"),
        ("NumpadMultiply", "*"),
        ("NumpadPlus", "+"),
        ("NumpadMinus", "-"),
        ("NumpadPeriod", "Num Decimal"),
        ("NumpadEquals", ""), // this shit does not exist idk what unity is cooking
        ("Numpad0", "Num 0"),
        ("Numpad1", "Num 1"),
        ("Numpad2", "Num 2"),
        ("Numpad3", "Num 3"),
        ("Numpad4", "Num 4"),
        ("Numpad5", "Num 5"),
        ("Numpad6", "Num 6"),
        ("Numpad7", "Num 7"),
        ("Numpad8", "Num 8"),
        ("Numpad9", "Num 9"),
        ("F1", "F1"),
        ("F2", "F2"),
        ("F3", "F3"),
        ("F4", "F4"),
        ("F5", "F5"),
        ("F6", "F6"),
        ("F7", "F7"),
        ("F8", "F8"),
        ("F9", "F9"),
        ("F10", "F10"),
        ("F11", "F11"),
        ("F12", "F12"),
        ("OEM1", ""),
        ("OEM2", ""),
        ("OEM3", ""),
        ("OEM4", ""),
        ("OEM5", ""),
        ("IMESelected", "")
    };

    private string GetKeyboardInputNameFromName(string inputName)
    {
        foreach (var pair in stringPairs)
        {
            if (pair.Item2 == inputName)
            {
                Debug.Log($"{pair.Item1} == {inputName}");
                return pair.Item1;
            }
        }
        return null;
    }

    private void Update()
    {
        // _keyboardState.Press((Key)System.Enum.Parse(typeof(Key), KeyRebinding.GetBindingName(_cutsceneActions[_actionToRebind]._inputActionReference, _cutsceneActions[_actionToRebind]._actionIndex)));

        if (Input.anyKey && _completedRebind)
        {
            _keyboardState.Press((Key)System.Enum.Parse(typeof(Key), _currentKeyboardInputIndex));
            InputSystem.QueueStateEvent(_keyboard, _keyboardState);
            // _keyboardState.Release((Key)System.Enum.Parse(typeof(Key), _currentKeyboardInputIndex.ToString()));
            // InputSystem.QueueStateEvent(_keyboard, _keyboardState);
        }

        if (test)
        {
            test = false;
            _isRebinding = false;
            _completedRebind = false;
            StartNewRebind(0);
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

    // var Gamepad = InputSystem.GetDevice<Gamepad>();
    // var GamepadState = new GamepadState();

    // Gamepad.rightStick.up.IsPressed();
    // Gamepad.rightStick.down.IsPressed();
    // Gamepad.rightStick.left.IsPressed();
    // Gamepad.rightStick.right.IsPressed();

    // Gamepad.leftStick.up.IsPressed();
    // Gamepad.leftStick.down.IsPressed();
    // Gamepad.leftStick.left.IsPressed();
    // Gamepad.leftStick.right.IsPressed();

    // InputSystem.QueueStateEvent(Gamepad, GamepadState);

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
