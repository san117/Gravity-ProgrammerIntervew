using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private ItemFlags _whiteList;

    [Header("Components")]
    [SerializeField] private Image _icon;

    protected ItemModel _currentItem;
    public bool IsEmpty => _currentItem == null;
    public ItemModel CurrentItem => _currentItem;

    public delegate void OnSetItem(ItemModel itemModel);
    public event OnSetItem _onSetItemEvent;

    [SerializeField] private UnityEvent _onSetItem;
    [SerializeField] private UnityEvent _onUnsetItem;
    #endregion

    #region Unity
    #endregion

    #region Public
    public void SetItem(ItemModel model)
    {
        _currentItem = model;

        if (_currentItem)
            _onSetItem.Invoke();
        else
            _onUnsetItem.Invoke();

        _onSetItemEvent?.Invoke(model);

        Paint();
    }

    public bool IsAllowed(ItemModel item)
    {
        return (_whiteList & item.Flags) != 0;
    }
    #endregion

    #region Internal
    private void Paint()
    {
        if(_currentItem != null)
        {
            _icon.sprite = _currentItem.Icon;
            _icon.color = Color.white;
        }
        else
        {
            _icon.sprite = null;
            _icon.color = Color.clear;
        }
    }
    #endregion
}
