using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ItemFlags
{
    None = 0,
    CHEST = 1 << 0,
    PANTS = 1 << 1,
    HAT = 1 << 2,
    COMMON = 1 << 3,
}

[CreateAssetMenu]
public class ItemModel : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _desc;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _equipLayerMask;
    [SerializeField] private ItemFlags _flags;

    public string Title => _title;
    public string Description => _desc;
    public Sprite Icon => _icon;
    public ItemFlags Flags => _flags;
    public int EquipLayerMask => _equipLayerMask;
}
