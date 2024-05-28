using System.Collections.Generic;

enum CharStates
{
    COMBAT,

    FREELOOK,
    TARGET,

    GROUNDED,
    SLOPED,
    AIRBORNE,
    JUMPING,

    IDLING,
    WALKING,
    RUNNING,
    DASHING,

    TEST,
}

public class CharStateFactory
{
    CharStateMachine _context;
    Dictionary<CharStates, CharBaseState> _states = new Dictionary<CharStates, CharBaseState>();

    public CharStateFactory(CharStateMachine currentContext)
    {
        _context = currentContext;
        _states[CharStates.COMBAT] = new CharCombatState(_context, this);

        _states[CharStates.TARGET] = new CharTargetState(_context, this);
        _states[CharStates.FREELOOK] = new CharFreeLookState(_context, this);

        _states[CharStates.GROUNDED] = new CharGroundedState(_context, this);
        _states[CharStates.SLOPED] = new CharSlopedState(_context, this);
        _states[CharStates.AIRBORNE] = new CharAirborneState(_context, this);
        _states[CharStates.JUMPING] = new CharJumpState(_context, this);

        _states[CharStates.IDLING] = new CharIdleState(_context, this);
        _states[CharStates.WALKING] = new CharWalkState(_context, this);
        _states[CharStates.RUNNING] = new CharRunState(_context, this);
        _states[CharStates.DASHING] = new CharDashState(_context, this);

        _states[CharStates.TEST] = new TestState(_context, this);

        // _states[CharStates.ULTRA] = new TestUltraState(_context, this);
        // _states[CharStates.SUPER] = new TestSuperState(_context, this);
        // _states[CharStates.SUB] = new TestSubState(_context, this);
    }

    public CharBaseState Test()
    {
        return _states[CharStates.TEST];
    }

    #region UltraStates

    public CharBaseState Combat()
    {
        return _states[CharStates.COMBAT];
    }
    #endregion

    #region SuperStates

    public CharBaseState Target()
    {
        return _states[CharStates.TARGET];
    }
    public CharBaseState FreeLook()
    {
        return _states[CharStates.FREELOOK];
    }
    #endregion

    #region MediumStates

    public CharBaseState Grounded()
    {
        return _states[CharStates.GROUNDED];
    }
    public CharBaseState Sloped()
    {
        return _states[CharStates.SLOPED];
    }
    public CharBaseState Airborne()
    {
        return _states[CharStates.AIRBORNE];
    }
    public CharBaseState Jumping()
    {
        return _states[CharStates.JUMPING];
    }
    #endregion

    #region SubStates

    public CharBaseState Walking()
    {
        return _states[CharStates.WALKING];
    }
    public CharBaseState Running()
    {
        return _states[CharStates.RUNNING];
    }
    public CharBaseState Idle()
    {
        return _states[CharStates.IDLING];
    }
    public CharBaseState Dashing()
    {
        return _states[CharStates.DASHING];
    }
    #endregion

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
