using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BigListTab : MonoBehaviour
{
     [SerializeField] private VirtualList list;
     [SerializeField] private int itemsCount;

    private void OnEnable() => list.BindSequential(itemsCount);

}
