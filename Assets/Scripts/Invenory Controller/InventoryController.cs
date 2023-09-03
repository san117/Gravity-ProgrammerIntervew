using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class InventoryController : MonoBehaviour
{
    private const int WIDTH = 4;
    private const int MAX_EQUIPMENT_SLOTS = 2;

    #region Fields
    [Header("Settings")]
    [SerializeField] private InventoryModel _playerInventory;
    [SerializeField] private InventoryModel _playerEquip;

    [Header("Components")]
    [SerializeField] private InventorySlotView[] _equipSlots;
    [SerializeField] private InventorySlotView[] _slots;
    [SerializeField] private Image _draggedItem_image;

    private bool _isDisplaying;
    private Vector2 _pointerPos;

    private Animator _anim;

    public bool IsDisplaying => _isDisplaying;

    private class DragItemInfo
    {
        public ItemModel draggedItem;
        public InventorySlotView from;

        public DragItemInfo(ItemModel draggedItem, InventorySlotView from)
        {
            this.draggedItem = draggedItem;
            this.from = from;
        }
    }

    private DragItemInfo _dragItemInfo;

    #endregion

    #region Unity
    private void Start()
    {
        _anim = GetComponent<Animator>();

        SetDisplay(false);
    }
    
    private void Update()
    {
        if (_dragItemInfo != null)
        {
            _draggedItem_image.gameObject.SetActive(true);
            _draggedItem_image.sprite = _dragItemInfo.draggedItem.Icon;

            Ray ray = Camera.main.ScreenPointToRay(_pointerPos);

            List<RaycastResult> raycastResults = new List<RaycastResult>();

            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
            {
                position = _pointerPos
            }, raycastResults);

            if (raycastResults.Count > 0)
            {
                InventorySlotView inventorySlotViewComponent = raycastResults
                        .Select(result => result.gameObject.GetComponent<InventorySlotView>())
                        .FirstOrDefault(component => component != null);

                if (inventorySlotViewComponent != null)
                {
                    if (_dragItemInfo != null)
                        if (inventorySlotViewComponent is InventorySlotSell)
                            (inventorySlotViewComponent as InventorySlotSell).Appraise(_dragItemInfo.draggedItem);
                }
            }
        }
        else
        {
            _draggedItem_image.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InitInventory();
    }

    private void OnDisable()
    {
        var allInventoryItems = _slots.Select(x => x.CurrentItem).ToList();
        _playerInventory.inventory = allInventoryItems;

        if (_playerEquip != null)
        {
            var allEquipment = _equipSlots.Select(x => x.CurrentItem).ToList();
            _playerEquip.inventory = allEquipment;
        }
    }
    #endregion

    #region Public
    public void SetDisplay(bool isActive)
    {
        _isDisplaying = isActive;

        if(_anim != null)
            _anim.SetBool("IsOpen", _isDisplaying);
    }

    public void StartDrag(InventorySlotView slot)
    {
        if(slot.CurrentItem != null)
        {
            _dragItemInfo = new DragItemInfo(slot.CurrentItem, slot);
            slot.SetItem(null);
        }
    }

    public void Drag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        _pointerPos = pointerData.position;
        _draggedItem_image.transform.position = _pointerPos; 
    }

    public void EndDrag(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;

        Ray ray = Camera.main.ScreenPointToRay(pointerData.position);

        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        {
            position = pointerData.position
        }, raycastResults);

        if(raycastResults.Count == 0)
        {
            if (_dragItemInfo != null)
                _dragItemInfo.from.SetItem(_dragItemInfo.draggedItem);
        }
        else
        {
            if(raycastResults.Count > 0)
            {
                InventorySlotView inventorySlotViewComponent = raycastResults
                    .Select(result => result.gameObject.GetComponent<InventorySlotView>())
                    .FirstOrDefault(component => component != null);

                if (inventorySlotViewComponent != null)
                {
                    if (inventorySlotViewComponent != null)
                    {
                        if (inventorySlotViewComponent is InventorySlotTrash)
                        {
                            if (_dragItemInfo != null)
                                inventorySlotViewComponent.SetItem(_dragItemInfo.draggedItem);
                        }
                        else if (inventorySlotViewComponent is InventorySlotSell)
                        {
                            if (_dragItemInfo != null)
                            {
                                var sellingSlot = (inventorySlotViewComponent as InventorySlotSell);

                                if (sellingSlot.CanBeSelled(_dragItemInfo.draggedItem))
                                {
                                    (inventorySlotViewComponent as InventorySlotSell).SellItem(_dragItemInfo.draggedItem);
                                    inventorySlotViewComponent.SetItem(null);
                                }
                                else
                                {
                                    _dragItemInfo.from.SetItem(_dragItemInfo.draggedItem);
                                }
                            }
                        }
                        else
                        {
                            if (_dragItemInfo != null)
                            {
                                if (inventorySlotViewComponent.IsAllowed(_dragItemInfo.draggedItem))
                                {
                                    if (inventorySlotViewComponent.IsEmpty)
                                    {
                                        inventorySlotViewComponent.SetItem(_dragItemInfo.draggedItem);
                                    }
                                    else
                                    {
                                        MoveItem(_dragItemInfo.draggedItem, _dragItemInfo.from, inventorySlotViewComponent);
                                    }
                                }
                                else
                                {
                                    _dragItemInfo.from.SetItem(_dragItemInfo.draggedItem);
                                }

                                _dragItemInfo = null;
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (_dragItemInfo != null)
                            _dragItemInfo.from.SetItem(_dragItemInfo.draggedItem);
                    }
                }
            }
        }

        _dragItemInfo = null;
    }

    public void AddItem(ItemModel model)
    {
        if (HasAviableSpace())
        {
            var emptySlot = FindFirstEmptySlot();
            emptySlot?.SetItem(model);
        }
    }

    public void MoveItem(ItemModel model, InventorySlotView from, InventorySlotView to)
    {
        var aux = to.CurrentItem;

        to.SetItem(model);
        from.SetItem(aux);
    }

    public bool HasAviableSpace()
    {
        return true;
    }

    public ItemModel GetItem(int x, int y)
    {
        return GetSlot(x, y).CurrentItem;
    }

    public InventorySlotView GetSlot(int x, int y)
    {
        int index = x + y * WIDTH;

        if (index >= 0 && index < _slots.Length)
        {
            return _slots[index];
        }

        return null;
    }

    public bool HasItem(int x, int y)
    {
        int index = x + y * WIDTH;

        if (index >= 0 && index < _slots.Length)
        {
            InventorySlotView inventorySlot = _slots[index];

            return !inventorySlot.IsEmpty;
        }

        return false;
    }
    #endregion

    #region Internal

    private InventorySlotView FindFirstEmptySlot()
    {
        for (int y = 0; y < WIDTH; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                var slot = GetSlot(x, y);
                if (slot.IsEmpty)
                    return slot;
            }
        }

        return null;
    }

    private void InitInventory()
    {
        for (int i = 0; i < _playerInventory.inventory.Count; i++)
        {
            if (i == _slots.Length)
                break;

            _slots[i].SetItem(_playerInventory.inventory[i]);
        }

        if (_playerEquip != null)
        {
            for (int i = 0; i < _playerEquip.inventory.Count; i++)
            {
                _equipSlots[i].SetItem(_playerEquip.inventory[i]);

                if (i == MAX_EQUIPMENT_SLOTS - 1)
                    break;
            }
        }
    }
    #endregion
}
