using UnityEngine;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

namespace Domain.Loggin
{
    /// <summary>
    /// Simple unified logging system for both Editor and runtime builds.  
    /// In the Unity Editor, logs are sent to the console.  
    /// In builds, logs are written to a persistent file on disk.
    /// </summary>
    public static class Logger
    {
        private static readonly string logFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "log.txt"
        );
        private static readonly StreamWriter writer = new StreamWriter(logFilePath, append: true)
        {
            AutoFlush = true
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Application.quitting += Close;
        }
        
        [Conditional("LOG_FILE")]
        public static void LogToFile(string message)
        {
            writer.WriteLine($"[{System.DateTime.Now:HH:mm:ss}] {message}");
        }

        [Conditional("LOG_UNITY")]
        public static void LogUnity(string message)
        {
            Debug.Log($"[LOG] {message}");
        }

        public static void Log(string message)
        {
#if UNITY_EDITOR
            LogUnity(message);
#else
            LogToFile(message);
#endif
        }

        public static void Close() => writer?.Close();
    }
}