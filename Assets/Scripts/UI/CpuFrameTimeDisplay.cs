using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;
using Logger = Domain.Loggin.Logger;

/// <summary>
/// Displays CPU frame time (excluding VSync wait) on top of all scenes.
/// Loaded once via Addressables and kept alive globally.
/// </summary>
public class CpuFrameTimeDisplay : MonoBehaviour
{
   private static CpuFrameTimeDisplay _instance;
   
   [SerializeField] private TextMeshProUGUI text;

   private static Recorder _main;
   private static Recorder _waitVsync;
   
   private const double NANO_TO_MILLI = 1e-6;

   public static async void Show()
   {
      if(_instance) return;

      var handle = Addressables.InstantiateAsync(AddressableKeys.CpuOverlay);
      await handle.Task;

      if (handle.Status != AsyncOperationStatus.Succeeded)
      {
         Logger.Log("Failed to load CpuOverlay prefab via Addressables.");
         return;
      }
      _instance = handle.Result.GetComponent<CpuFrameTimeDisplay>();
      DontDestroyOnLoad(_instance.gameObject);
   }

   private void Awake()
   {
      if (_instance != null && _instance != this)
      {
         Destroy(gameObject);
         return;
      }
      _instance = this;
      _main = Recorder.Get("Main Thread");
      _waitVsync = Recorder.Get("WaitForTargetFPS");
      _main.enabled = _waitVsync.enabled = true;
   }

   private void Update()
   {
      double cpuMs = (_main.elapsedNanoseconds - _waitVsync.elapsedNanoseconds) * NANO_TO_MILLI;
      text.SetText($"CPU Frame Time: {cpuMs:0.00} ms");
   }

}
