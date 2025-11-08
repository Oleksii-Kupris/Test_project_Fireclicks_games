using UnityEngine;

namespace Infrastructure.Storage
{
    /// <summary>
    /// Simple wrapper around Unity's PlayerPrefs providing a unified storage interface.  
    /// Used for persistent key-value data such as tokens or configuration values.
    /// </summary>
    public class PlayerPrefsStorage : IStorage
    {
        public bool HasKey(string key) => PlayerPrefs.HasKey(key);
      
        public void SetValue(string key, string value)
        {
           PlayerPrefs.SetString(key, value);
           PlayerPrefs.Save();
        }

        public string GetValue(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
        
    }
}