using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryModel : ScriptableObject
{
    public List<ItemModel> inventory = new List<ItemModel>();
}
