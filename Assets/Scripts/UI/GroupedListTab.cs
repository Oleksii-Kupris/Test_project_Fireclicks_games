using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Domain.Loggin.Logger;

/// <summary>
/// Controls the grouped rarity list tab.
/// Builds a repeating rarity pattern and binds it to the virtualized list.
/// </summary>
public class GroupedListTab : MonoBehaviour
{
    [SerializeField] private VirtualList virtualList;
    [SerializeField] private RarityDatabase rarityDatabase;

    // Cached collections to avoid allocations during pattern building.
    private readonly Dictionary<int, int> _remaining = new Dictionary<int, int>();
    private readonly List<int> _result = new List<int>();
    private readonly List<int> _keysBuffer = new List<int>();

    private void OnEnable()
    {
        if (rarityDatabase == null)
        {
            Logger.Log("Rarity database is null");
            return;
        }

        var data = BuildPattern(rarityDatabase.Rarities);
        virtualList.BindData(data, rarityDatabase);
    }

    /// <summary>
    /// Generates a sequential pattern of rarity codes based on their counts.
    /// Example order: 1, 2, 3, 1, 2, 3, 1, 2, ...
    /// </summary>
    private List<int> BuildPattern(IReadOnlyList<RarityDatabase.RarityEntry> rarities)
    {
        _result.Clear();
        _keysBuffer.Clear();
        _remaining.Clear();

        for (int i = 0; i < rarities.Count; i++)
        {
            var r= rarities[i];
            if (r.count > 0)
            {
                _remaining[r.code] = r.count;
                _keysBuffer.Add(r.code);
            }
        }

        bool hasRemaining = true;

        while (hasRemaining)
        {
            hasRemaining = false;
            for (int i = 0; i < _keysBuffer.Count; i++)
            {
                int key = _keysBuffer[i];
                int left = _remaining[key];

                if (left > 0)
                {
                    _result.Add(key);
                    _remaining[key] = left - 1;
                    hasRemaining = true;
                }
            }
        }
        return _result;
    }

    public bool TryGetRarityInfo(int code, out RarityDatabase.RarityEntry entry)
    {
        return rarityDatabase.TryGetRarity(code, out entry);
    }
}