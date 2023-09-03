using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class MercancyInfo
    {
        public int buyPrice;
        public int sellPrice;
        public ItemModel item;
    }

    #region Fields
    [Header("Settings")]
    [SerializeField] private MercancyInfo[] _aviableMercancy;

    [Header("Components")]
    [SerializeField] private MercancySlot _mercancyTemplate;
    [SerializeField] private Transform _content;
    [SerializeField] private TextMeshProUGUI _npcDialogue;
    [SerializeField] private InventoryController _internalInventory;
    [Header("Components / Tooltip")]
    [SerializeField] private TextMeshProUGUI _tooltip_tile;
    [SerializeField] private TextMeshProUGUI _tooltip_desc;
    [SerializeField] private TextMeshProUGUI _tooltip_sellingPrice;

    private bool _updatingTooltip;

    #endregion

    #region Unity
    private void OnEnable()
    {
        UpdateMercancyView();
    }

    private void Update()
    {
        if (_updatingTooltip)
        {
            _updatingTooltip = false;
        }
        else
        {
            ClearTooltip();
        }
    }
    #endregion

    #region Public
    public void Buy(MercancyInfo mercancyInfo)
    {
        string[] dialogues = new string[5]
        {
            "Thank you! It's one of my best works",
            "I hope my work serves you well on your journey",
            "I'm always here to provide quality craftsmanship",
            "Feel free to return if you ever require my services again",
            "I hope you find this item to be of great use"
        };

        _npcDialogue.text = dialogues[Random.Range(0, dialogues.Length - 1)];

        PlayerController.Singleton.SubstractMoney(mercancyInfo.buyPrice);

        _internalInventory.AddItem(mercancyInfo.item);

        UpdateMercancyView();
    }

    public void Sell(ItemModel model)
    {
        if (CanSell(model, out var mercancyInfo))
        {
            PlayerController.Singleton.AddMoney(mercancyInfo.sellPrice);
        }
    }

    public bool CanSell(ItemModel model)
    {
        foreach (var mercany in _aviableMercancy)
        {
            if (mercany.item.Title == model.Title)
                return true;
        }

        return false;
    }

    public bool CanSell(ItemModel model, out MercancyInfo mercancyInfo)
    {
        foreach (var mercany in _aviableMercancy)
        {
            mercancyInfo = mercany;

            if (mercany.item.Title == model.Title)
                return true;
        }

        mercancyInfo = null;

        return false;
    }

    public void PaintTooltip(ItemModel model)
    {
        _tooltip_tile.text = model.Title;
        _tooltip_desc.text = model.Description;
        _tooltip_sellingPrice.text = CanSell(model, out var mercancyInfo) ? "$" + mercancyInfo.sellPrice : "<color=#FF5E00>You can't sell this item here</color>";
        _updatingTooltip = true;

    }
    #endregion

    #region Internal
    private void UpdateMercancyView()
    {
        foreach (Transform child in _content)
        {
            if (child.gameObject != _mercancyTemplate.gameObject)
                Destroy(child.gameObject);
        }

        foreach (var mercancy in _aviableMercancy)
        {
            var slot = Instantiate(_mercancyTemplate, _content);

            var playerHasSpace = _internalInventory.HasAviableSpace();
            var playerHasMoney = PlayerController.Singleton.HasEnoughMoney(mercancy.buyPrice);

            slot.SetMercancy(mercancy, playerHasSpace && playerHasMoney);
        }
    }

    private void ClearTooltip()
    {
        _tooltip_tile.text = "";
        _tooltip_desc.text = "Drop item here to sell";
        _tooltip_sellingPrice.text = "";
    }
    #endregion
}
