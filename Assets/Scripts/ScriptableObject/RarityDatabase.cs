using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores rarity configuration data for list rendering,
/// including color, name, and item count for each rarity type.
/// </summary>
[CreateAssetMenu(fileName = "RarityDatabase", menuName = "Scriptable Objects/RarityDatabase")]
public class RarityDatabase : ScriptableObject
{
    [System.Serializable]
    public class RarityEntry
    {
        public int code;
        public string name;
        public Color color;
        public int count;
    }

    [SerializeField] private List<RarityEntry> rarities = new();
    
    public IReadOnlyList<RarityEntry> Rarities => rarities;

    public bool TryGetRarity(int code, out RarityEntry entry)
    {
        var list = rarities;
        for (int i = 0, count = list.Count; i < count; i++)
        {
            var r = list[i];
            if (r.code == code)
            {
                entry = r;
                return true;
            }
        }

        entry = null;
        return false;
    }
}
