using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Virtualized scroll list that reuses a fixed pool of UI items.
/// Supports sequential or data-driven binding (e.g. rarity list).
/// </summary>
public class VirtualList : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private VirtualListItem itemPrefab;

    [SerializeField] private int visibleCount = 3;

    private readonly List<VirtualListItem> _pool = new List<VirtualListItem>();
    private float _itemHeight;
    private int _first;
    private int _count;
    private bool _initialized;

    private List<int> _data;
    private RarityDatabase _rarityDatabase;

    private void Initialize()
    {
        // Ensures canvas is updated before viewport measurement
        if (scrollRect.viewport.rect.height == 0)
            Canvas.ForceUpdateCanvases();

        float viewportHeight = scrollRect.viewport.rect.height;
        _itemHeight = viewportHeight / visibleCount;

        var prefabRect = (RectTransform) itemPrefab.transform;
        prefabRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _itemHeight);

        for (int i = 0; i < visibleCount; i++)
        {
            var item = Instantiate(itemPrefab, content);
            _pool.Add(item);
        }

        content.anchoredPosition = Vector2.zero;
        scrollRect.onValueChanged.AddListener(OnScroll);
        _initialized = true;
    }

    private void OnScroll(Vector2 _) => UpdateItems();

    public void BindSequential(int count)
    {
        if (!_initialized)
            Initialize();

        _data = null;
        _rarityDatabase = null;
        _count = count;
        UpdateContentSize();
        _first = -1;
        UpdateItems(true);
    }

    public void BindData(List<int> data, RarityDatabase database)
    {
        if (!_initialized)
            Initialize();

        _data = data;
        _rarityDatabase = database;
        _count = data.Count;
        UpdateContentSize();
        _first = -1;
        UpdateItems(true);
    }

    private void UpdateContentSize()
    {
        float contentH = _count * _itemHeight;
        content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Ceil(contentH));
    }

    private void UpdateItems(bool force = false)
    {
        if (_count == 0)
            return;

        int maxFirst = Mathf.Max(0, _count - _pool.Count);
        int first = Mathf.Clamp(
            Mathf.FloorToInt(content.anchoredPosition.y / _itemHeight),
            0, maxFirst
        );

        if (!force && first == _first)
            return;

        _first = first;

        for (int i = 0; i < _pool.Count; i++)
        {
            int index = _first + i;
            var item = _pool[i];

            if (index >= _count)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            item.gameObject.SetActive(true);
            var rt = (RectTransform) item.transform;
            rt.anchoredPosition = new Vector2(0f, -index * _itemHeight);

            if (_data == null)
                item.SetTextInt(index);
            else
                item.SetTextColored(_data[index], _rarityDatabase);
        }
    }

    private void OnDestroy()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);
    }
}