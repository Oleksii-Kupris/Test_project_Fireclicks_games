using TMPro;
using UnityEngine;

/// <summary>
/// Represents a single UI element within a virtualized scroll list.  
/// Handles simple integer display or color-coded text using rarity data.
/// </summary>
public class VirtualListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    /// <summary>
    /// Displays a numeric value inside the list item.
    /// </summary>
    public void SetTextInt(int value)
    {
        text.SetText("{0}", value);
    }

    /// <summary>
    /// Displays a rarity-based value using color and data from the provided <see cref="RarityDatabase"/>.
    /// </summary>
    /// <param name="rarity">Rarity code to look up in the database.</param>
    /// <param name="rarityDatabase">Database containing rarity names and colors.</param>
    public void SetTextColored(int rarity, RarityDatabase rarityDatabase)
    {
        if (rarityDatabase == null)
        {
            text.SetText("No DB");
            text.color = Color.gray;
            return;
        }

        if (rarityDatabase.TryGetRarity(rarity, out var entry))
        {
            text.SetText($"{entry.code}");
            text.color = entry.color;
        }
        else
        {
            text.SetText("Unknown");
            text.color = Color.white;
        }
    }
}