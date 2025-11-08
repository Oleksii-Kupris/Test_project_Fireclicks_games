using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Logger = Domain.Loggin.Logger;

namespace StateMachine
{
    /// <summary>
    /// Game state responsible for loading the main gameplay scene asynchronously.  
    /// Displays a loading screen and transitions to <see cref="GameLoopState"/> once the scene is ready.
    /// </summary>
    public class LoadingState : IGameState
    {
        private readonly ILogger _logger;

        public async Task Enter(GameStateMachine gameStateMachine)
        {
            GameObject loadingGO = await Addressables.InstantiateAsync(AddressableKeys.LoadingCanvas).Task;
            var ui = loadingGO.GetComponentInChildren<LoadingScreen>();

            var sceneHandle = Addressables.LoadSceneAsync(AddressableKeys.GameLoop, LoadSceneMode.Single);
            while (!sceneHandle.IsDone)
            {
                ui.SetProgress(sceneHandle.PercentComplete);
                await Task.Yield();
            }
            
            Object.Destroy(loadingGO);
            Logger.Log("GameLoop scene loaded");
            await gameStateMachine.Enter<GameLoopState>();
        }

        public Task Exit() => Task.CompletedTask;
       
    }
}
