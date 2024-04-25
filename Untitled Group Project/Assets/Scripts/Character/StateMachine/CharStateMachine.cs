using UnityEngine;

public class CharStateMachine : MonoBehaviour
{
    CharStateFactory _states;

    CharBaseState _currentState;
    public CharBaseState CurrentState
    { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        _states = new CharStateFactory(this);
        _currentState = _states.Ultra();
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateStates();
    }
}