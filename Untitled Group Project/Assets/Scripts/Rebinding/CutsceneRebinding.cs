using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.HID.HID;
using System.Collections;

public class CutsceneRebinding : MonoBehaviour
{

    private Keyboard _keyboard;
    private KeyboardState _keyboardState;

    [SerializeField] List<Keybind> _cutsceneActions;

    [SerializeField] bool _completedRebind;
    [SerializeField] bool _isRebinding;
    [SerializeField] bool _startedRebind;

    [SerializeField] bool _newInput;

    [SerializeField] int _bindingIndex;

    [SerializeField] string _lastPressedKey;
    [SerializeField] string _currentKeyboardInputIndex;

    [SerializeField] string _currentPressedKey;
    [SerializeField] string _previousPressedKey;

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
        KeyRebinding.rebindStarted += StartRebind;
    }

    private void OnDisable()
    {
        KeyRebinding.rebindComplete -= CompleteRebind;
        KeyRebinding.rebindStarted -= StartRebind;
    }

    private void StartRebind(InputAction action, int arg2)
    {
        _startedRebind = true;
    }

    void CompleteRebind()
    {
        Debug.Log($"Rebind has been completed with binding: ( {KeyRebinding.GetBindingName(_cutsceneActions[_bindingIndex]._inputActionReference, _cutsceneActions[_bindingIndex]._actionIndex)} )");

        _completedRebind = true;

        _startedRebind = false;

        _isRebinding = false;

        _newInput = false;

        _currentKeyboardInputIndex = GetKeyboardItem1FromItem2(KeyRebinding.GetBindingName(_cutsceneActions[_bindingIndex]._inputActionReference, _cutsceneActions[_bindingIndex]._actionIndex));
    }

    private void Start()
    {
        StartNewRebind(0);
    }

    private List<(string, string, KeyCode)> stringPairs = new List<(string, string, KeyCode)>
    {
        ("None", "", KeyCode.None),
        ("Space", "Space", KeyCode.Space),
        ("Enter", "Enter", KeyCode.Return),
        ("Tab", "Tab", KeyCode.Tab),
        ("Backquote", "Grave", KeyCode.BackQuote),
        ("Backquote", "``", KeyCode.BackQuote),
        ("Quote", "''", KeyCode.Quote),
        ("Quote", "Acute", KeyCode.Quote),
        ("Semicolon", ";", KeyCode.Semicolon),
        ("Comma", ",", KeyCode.Comma),
        ("Period", ".", KeyCode.Period),
        ("Slash", "/", KeyCode.Slash),
        ("Backslash", "\\", KeyCode.Backslash),
        ("LeftBracket", "[", KeyCode.LeftBracket),
        ("RightBracket", "]", KeyCode.RightBracket),
        ("Minus", "-", KeyCode.Minus),
        ("Equals", "=", KeyCode.Equals),
        ("A", "A", KeyCode.A),
        ("B", "B", KeyCode.B),
        ("C", "C", KeyCode.C),
        ("D", "D", KeyCode.D),
        ("E", "E", KeyCode.E),
        ("F", "F", KeyCode.F),
        ("G", "G", KeyCode.G),
        ("H", "H", KeyCode.H),
        ("I", "I", KeyCode.I),
        ("J", "J", KeyCode.J),
        ("K", "K", KeyCode.K),
        ("L", "L", KeyCode.L),
        ("M", "M", KeyCode.M),
        ("N", "N", KeyCode.N),
        ("O", "O", KeyCode.O),
        ("P", "P", KeyCode.P),
        ("Q", "Q", KeyCode.Q),
        ("R", "R", KeyCode.R),
        ("S", "S", KeyCode.S),
        ("T", "T", KeyCode.T),
        ("U", "U", KeyCode.U),
        ("V", "V", KeyCode.V),
        ("W", "W", KeyCode.W),
        ("X", "X", KeyCode.X),
        ("Y", "Y", KeyCode.Y),
        ("Z", "Z", KeyCode.Z),
        ("Digit1", "1", KeyCode.Alpha1),
        ("Digit2", "2", KeyCode.Alpha2),
        ("Digit3", "3", KeyCode.Alpha3),
        ("Digit4", "4", KeyCode.Alpha4),
        ("Digit5", "5", KeyCode.Alpha5),
        ("Digit6", "6", KeyCode.Alpha6),
        ("Digit7", "7", KeyCode.Alpha7),
        ("Digit8", "8", KeyCode.Alpha8),
        ("Digit9", "9", KeyCode.Alpha9),
        ("Digit0", "0", KeyCode.Alpha0),
        ("LeftShift", "Shift", KeyCode.LeftShift),
        ("RightShift", "Right Shift", KeyCode.RightShift),
        ("LeftAlt", "Alt", KeyCode.LeftAlt),
        ("RightAlt", "Right Alt", KeyCode.RightAlt),
        ("AltGr", "Right Alt", KeyCode.AltGr),
        ("LeftCtrl", "Ctrl", KeyCode.LeftControl),
        ("LeftWindows", "Left Windows", KeyCode.LeftWindows),
        ("RightWindows", "Right Windows", KeyCode.RightWindows),
        ("RightCtrl", "Right Ctrl", KeyCode.RightControl),
        ("LeftMeta", "Left Windows", KeyCode.LeftCommand), // macOS Command key
        ("RightMeta", "Right Windows", KeyCode.RightCommand), // macOS Command key
        ("LeftApple", "Left Windows", KeyCode.LeftCommand), // macOS Command key
        ("RightApple", "Right Windows", KeyCode.RightCommand), // macOS Command key
        ("LeftCommand", "Left Command", KeyCode.LeftCommand), // macOS Command key
        ("RightCommand", "Right Command", KeyCode.RightCommand), // macOS Command key
        ("ContextMenu", "Application", KeyCode.Menu),
        ("Escape", "Escape", KeyCode.Escape),
        ("LeftArrow", "Left", KeyCode.LeftArrow),
        ("RightArrow", "Right", KeyCode.RightArrow),
        ("UpArrow", "Up", KeyCode.UpArrow),
        ("DownArrow", "Down", KeyCode.DownArrow),
        ("Backspace", "Backspace", KeyCode.Backspace),
        ("PageDown", "Pgdown", KeyCode.PageDown),
        ("PageUp", "Pgup", KeyCode.PageUp),
        ("Home", "Home", KeyCode.Home),
        ("End", "End", KeyCode.End),
        ("Insert", "Insert", KeyCode.Insert),
        ("Delete", "Delete", KeyCode.Delete),
        ("CapsLock", "CapsLock", KeyCode.CapsLock),
        ("NumLock", "Num Lock", KeyCode.Numlock),
        ("PrintScreen", "Prnt Scrn", KeyCode.Print),
        ("ScrollLock", "Scroll Lock", KeyCode.ScrollLock),
        ("Pause", "Break", KeyCode.Pause),
        ("NumpadEnter", "Num Enter", KeyCode.KeypadEnter),
        ("NumpadDivide", "Num Divide", KeyCode.KeypadDivide),
        ("NumpadMultiply", "*", KeyCode.KeypadMultiply),
        ("NumpadPlus", "+", KeyCode.KeypadPlus),
        ("NumpadMinus", "-", KeyCode.KeypadMinus),
        ("NumpadPeriod", "Num Decimal", KeyCode.KeypadPeriod),
        ("NumpadEquals", "", KeyCode.KeypadEquals),
        ("Numpad0", "Num 0", KeyCode.Keypad0),
        ("Numpad1", "Num 1", KeyCode.Keypad1),
        ("Numpad2", "Num 2", KeyCode.Keypad2),
        ("Numpad3", "Num 3", KeyCode.Keypad3),
        ("Numpad4", "Num 4", KeyCode.Keypad4),
        ("Numpad5", "Num 5", KeyCode.Keypad5),
        ("Numpad6", "Num 6", KeyCode.Keypad6),
        ("Numpad7", "Num 7", KeyCode.Keypad7),
        ("Numpad8", "Num 8", KeyCode.Keypad8),
        ("Numpad9", "Num 9", KeyCode.Keypad9),
        ("F1", "F1", KeyCode.F1),
        ("F2", "F2", KeyCode.F2),
        ("F3", "F3", KeyCode.F3),
        ("F4", "F4", KeyCode.F4),
        ("F5", "F5", KeyCode.F5),
        ("F6", "F6", KeyCode.F6),
        ("F7", "F7", KeyCode.F7),
        ("F8", "F8", KeyCode.F8),
        ("F9", "F9", KeyCode.F9),
        ("F10", "F10", KeyCode.F10),
        ("F11", "F11", KeyCode.F11),
        ("F12", "F12", KeyCode.F12),
        ("OEM1", "", KeyCode.None), // OEM keys vary and need specific context
        ("OEM2", "", KeyCode.None), // OEM keys vary and need specific context
        ("OEM3", "", KeyCode.None), // OEM keys vary and need specific context
        ("OEM4", "", KeyCode.None), // OEM keys vary and need specific context
        ("OEM5", "", KeyCode.None), // OEM keys vary and need specific context
        ("IMESelected", "", KeyCode.None) // IME selection is context-specific
    };

    private string GetKeyboardItem1FromItem2(string possibleItem2)
    {
        foreach (var pair in stringPairs)
        {
            if (pair.Item2 == possibleItem2)
            {
                return pair.Item1;
            }
        }
        return null;
    }

    private KeyCode GetKeyboardItem3FromItem2(string possibleItem2)
    {
        foreach (var pair in stringPairs)
        {
            if (pair.Item2 == possibleItem2)
            {
                Debug.Log($"{pair.Item1} == {possibleItem2}");
                return pair.Item3;
            }
        }
        return KeyCode.None;
    }

    private void Update()
    {

        if (Input.anyKey && _completedRebind)
        {
            // THIS NEEDS TO USE THE NEW ONES SO IT WORKS WITH THAT
            _keyboardState.Press((Key)System.Enum.Parse(typeof(Key), _currentKeyboardInputIndex));
            InputSystem.QueueStateEvent(_keyboard, _keyboardState);
            // THIS NEEDS TO USE THE NEW ONES SO IT WORKS WITH THAT

            if (!_newInput)
            {
                foreach (var pair in stringPairs)
                {
                    if (Input.GetKey(pair.Item3))
                    {
                        if (_currentPressedKey == "")
                        {
                            _currentPressedKey = pair.Item2;
                            _previousPressedKey = pair.Item2;
                            Debug.Log("FIRST TIME KEY PRESSED");
                        }
                        if (_currentPressedKey != pair.Item2)
                        {
                            // THIS CURRENTLY LOOPS NEED TO FIND A WAT TO MAKE IT NOT LOOP
                            Debug.Log($"NEW INPUT {pair.Item2}");
                            _newInput = true;

                            _previousPressedKey = _currentPressedKey;
                            _currentPressedKey = pair.Item2;

                            // REBINDING NEEDS NEW INPUT SO WE SIMULATE A NEW ONE ( THE NEW INPUT THAT IS ALREADY BEING PRESSED CURRENTLY )
                            _keyboardState.Release((Key)System.Enum.Parse(typeof(Key), pair.Item1));
                            InputSystem.QueueStateEvent(_keyboard, _keyboardState);

                            _keyboardState.Press((Key)System.Enum.Parse(typeof(Key), pair.Item1));
                            InputSystem.QueueStateEvent(_keyboard, _keyboardState);
                            // REBINDING NEEDS NEW INPUT SO WE SIMULATE A NEW ONE ( THE NEW INPUT THAT IS ALREADY BEING PRESSED CURRENTLY )

                            StartNewRebind(_bindingIndex);
                            break; // Currently for not checking more needs a bigger solution later
                        }
                    }
                }
            }
        }
    }

    void StartNewRebind(int index)
    {
        if (!_isRebinding && !_startedRebind)
        {
            _isRebinding = true;
            _completedRebind = false;
            Debug.Log("REBIND");
            KeyRebinding.StartRebind(_cutsceneActions[index]._inputActionReference, _cutsceneActions[index]._excludeMouse, _cutsceneActions[index]._actionIndex);
        }
    }
}