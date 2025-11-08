using System.Threading.Tasks;
using Domain.Token;
using UnityEngine;
using Logger = Domain.Loggin.Logger;
namespace StateMachine
{
    /// <summary>
    /// Initial game state responsible for preparing persistent data  
    /// and generating a unique encrypted token for the player before loading the next scene.
    /// </summary>
    public class InitializationState : IGameState
    {
        private readonly ITokenService _tokens;
        
        public InitializationState(ITokenService tokens) => _tokens = tokens;
        public async Task Enter(GameStateMachine gameStateMachine)
        {
            _tokens.EnsureToken();
            await gameStateMachine.Enter<LoadingState>();
            Logger.Log("Initialization finished");
        }

     public Task Exit() => Task.CompletedTask;
    
    }
}
