using System;
using System.Collections.Generic;
using Domain.Token;
using Infrastructure.Encryption;
using Infrastructure.Storage;
using StateMachine;
using UnityEngine;

/// <summary>
/// Entry point of the application.  
/// Initializes core services, builds the game state machine,  
/// and enters the initial state on startup.
/// </summary>
public class Bootstrap : MonoBehaviour
{
   private async void Awake()
   {
      DontDestroyOnLoad(this);

      IStorage storage = new PlayerPrefsStorage();
      IEncryptor encryptor = new AesEncryptor(SharedSecrets.EncryptionKey);
      ITokenService tokens = new TokenService(storage, encryptor);

      var states = new Dictionary<Type, IGameState>
      {
          {typeof(InitializationState), new InitializationState(tokens)},
          {typeof(LoadingState), new LoadingState()},
          {typeof(GameLoopState), new GameLoopState()}
      };
      var fsm = new GameStateMachine(states);
      await fsm.Enter<InitializationState>();
   }
}
