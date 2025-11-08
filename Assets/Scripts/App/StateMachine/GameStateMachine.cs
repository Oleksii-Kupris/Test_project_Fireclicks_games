using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StateMachine;

namespace StateMachine
{
    /// <summary>
    /// Simple asynchronous state machine.
    /// Stores and manages registered game states and handles state transitions.
    /// </summary>
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;

        public GameStateMachine(Dictionary<Type, IGameState> states) => _states = states;

        public async Task Enter<T>() where T : IGameState
        {
            if (_states.TryGetValue(typeof(T), out var state))
                await state.Enter(this);
        }
    }
}