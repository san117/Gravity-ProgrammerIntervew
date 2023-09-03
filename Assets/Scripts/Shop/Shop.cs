using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

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
    [SerializeField] private CoinsVFX _coinsVFX;
    [SerializeField] private Transform _blacksmithNPC;
    [Header("Components / Tooltip")]
    [SerializeField] private TextMeshProUGUI _tooltip_tile;
    [SerializeField] private TextMeshProUGUI _tooltip_desc;
    [SerializeField] private TextMeshProUGUI _tooltip_sellingPrice;

    private bool _updatingTooltip;

    private bool _shopOpen;

    public bool IsOpen => _shopOpen;

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
        PlayerController.Singleton.DiplayBalance(-1);

        _internalInventory.SaveInventory();
        _internalInventory.RefreshInventory();
        UpdateMercancyView();

        var vfx = Instantiate(_coinsVFX);

        vfx.Init(mercancyInfo.buyPrice, 15, 100, MenuManager.Singleon.CoinBalancePosition, Camera.main.WorldToScreenPoint(_blacksmithNPC.position));
    }

    public void Sell(ItemModel model)
    {
        if (CanSell(model, out var mercancyInfo))
        {
            PlayerController.Singleton.AddMoney(mercancyInfo.sellPrice);
            PlayerController.Singleton.DiplayBalance(-1);

            _internalInventory.SaveInventory();
            _internalInventory.RefreshInventory();

            UpdateMercancyView();

            var vfx = Instantiate(_coinsVFX);
            
            vfx.Init(mercancyInfo.sellPrice, 15, 100, Mouse.current.position.ReadValue(), MenuManager.Singleon.CoinBalancePosition);
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

    public void OpenShop()
    {
        GetComponent<Animator>().SetBool("IsOpen", true);
        _shopOpen = true;

        PlayerController.Singleton.DiplayBalance(-1);
    }

    public void CloseShop()
    {
        GetComponent<Animator>().SetBool("IsOpen", false);
        _shopOpen = false;

        PlayerController.Singleton.DiplayBalance(1);
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
