using System.Threading.Tasks;

namespace StateMachine
{
    /// <summary>
    /// Main gameplay state.  
    /// Activates runtime performance overlay and runs the core game loop.
    /// </summary>
    public class GameLoopState : IGameState
    {
        public async Task Enter(GameStateMachine gameStateMachine)
        {
            CpuFrameTimeDisplay.Show();
            await Task.CompletedTask;
        }

        public Task Exit() => Task.CompletedTask;
    }
}