using System.Threading.Tasks;

namespace StateMachine
{
    public interface IGameState
    {
        Task Enter(GameStateMachine gameStateMachine);
        Task Exit();
    }
}