using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotSell : InventorySlotView
{
    [SerializeField] private Shop _shop;

    public void SellItem(ItemModel item)
    {
        _shop.Sell(item);
    }

    public void Appraise(ItemModel item)
    {
        _shop.PaintTooltip(item);
    }

    public bool CanBeSelled(ItemModel item)
    {
        return _shop.CanSell(item);
    }
}
