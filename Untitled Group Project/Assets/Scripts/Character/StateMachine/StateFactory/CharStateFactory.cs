using System.Collections.Generic;

enum CharStates
{
    // GROUND,
    ULTRA,
    SUPER,
    SUB,
}

public class CharStateFactory
{
    CharStateMachine _context;
    Dictionary<CharStates, CharBaseState> _states = new Dictionary<CharStates, CharBaseState>();

    public CharStateFactory(CharStateMachine currentContext)
    {
        _context = currentContext;
        // _states[CharStates.GROUND] = new CharGroundedState(_context, this);

        _states[CharStates.ULTRA] = new TestUltraState(_context, this);
        _states[CharStates.SUPER] = new TestSuperState(_context, this);
        _states[CharStates.SUB] = new TestSubState(_context, this);
    }

    // public CharBaseState Grounded()
    // {
    //     return _states[CharStates.GROUND];
    // }

    public CharBaseState Ultra()
    {
        return _states[CharStates.ULTRA];
    }

    public CharBaseState Super()
    {
        return _states[CharStates.SUPER];
    }

    public CharBaseState Sub()
    {
        return _states[CharStates.SUB];
    }
}
