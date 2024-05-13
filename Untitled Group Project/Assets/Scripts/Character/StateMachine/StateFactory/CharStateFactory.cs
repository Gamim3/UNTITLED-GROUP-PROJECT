using System.Collections.Generic;

enum CharStates
{
    GROUNDED,
    AIRBORNE,
    SLOPED,
    WALKING,
    JUMPING,
    IDLE,

    // ULTRA,
    // SUPER,
    // SUB,
}

public class CharStateFactory
{
    CharStateMachine _context;
    Dictionary<CharStates, CharBaseState> _states = new Dictionary<CharStates, CharBaseState>();

    public CharStateFactory(CharStateMachine currentContext)
    {
        _context = currentContext;
        _states[CharStates.GROUNDED] = new CharGroundedState(_context, this);
        _states[CharStates.WALKING] = new CharWalkState(_context, this);
        _states[CharStates.SLOPED] = new CharSlopedState(_context, this);
        _states[CharStates.JUMPING] = new CharJumpState(_context, this);
        _states[CharStates.AIRBORNE] = new CharAirborneState(_context, this);
        _states[CharStates.IDLE] = new CharIdleState(_context, this);

        // _states[CharStates.ULTRA] = new TestUltraState(_context, this);
        // _states[CharStates.SUPER] = new TestSuperState(_context, this);
        // _states[CharStates.SUB] = new TestSubState(_context, this);
    }

    public CharBaseState Grounded()
    {
        return _states[CharStates.GROUNDED];
    }
    public CharBaseState Walking()
    {
        return _states[CharStates.WALKING];
    }
    public CharBaseState Sloped()
    {
        return _states[CharStates.SLOPED];
    }
    public CharBaseState Jumping()
    {
        return _states[CharStates.JUMPING];
    }
    public CharBaseState Airborne()
    {
        return _states[CharStates.AIRBORNE];
    }
    public CharBaseState Idle()
    {
        return _states[CharStates.IDLE];
    }

    // public CharBaseState Ultra()
    // {
    //     return _states[CharStates.ULTRA];
    // }

    // public CharBaseState Super()
    // {
    //     return _states[CharStates.SUPER];
    // }

    // public CharBaseState Sub()
    // {
    //     return _states[CharStates.SUB];
    // }
}
