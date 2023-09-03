using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InventoryController _personalInventory;
    [SerializeField] private Shop _shop;


    private static MenuManager _singleton;

    public static MenuManager Singleon
    {
        get
        {
            if (_singleton == null)
                _singleton = FindAnyObjectByType<MenuManager>();

            return _singleton;
        }
    }

    public void OpenPersonalInventory()
    {
        _personalInventory.SetDisplay(true);
    }

    public void OpenShop()
    {
        _shop.OpenShop();
    }

    public void ClosePersonalInventory()
    {
        _personalInventory.SetDisplay(false);
    }

    public void CloseShop()
    {
        _shop.CloseShop();
    }
}
