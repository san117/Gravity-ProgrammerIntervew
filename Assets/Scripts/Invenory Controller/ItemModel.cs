using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemModel : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField] private string _desc;
    [SerializeField] private Sprite _icon;
}
