using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private InputActionAsset _playerControls;
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private InventoryController _personalInventory;
    [SerializeField] private Shop _shop;
    #endregion

    #region Unity
    void Start()
    {
        _playerControls.FindActionMap("Gameplay").FindAction("Inventory").performed += InventoryButtonPressed;
        _playerControls.FindActionMap("UI").FindAction("Inventory").performed += InventoryButtonPressed;

        _playerControls.FindActionMap("UI").Disable();
    }

    private void InventoryButtonPressed(InputAction.CallbackContext obj)
    {
        if (_shop.IsOpen) return;

        var isDisplaying = !_personalInventory.IsDisplaying;

        if (isDisplaying)
            MenuManager.Singleon.OpenPersonalInventory();
        else
            MenuManager.Singleon.ClosePersonalInventory();

        if(isDisplaying)
        {
            _playerControls.FindActionMap("Gameplay").Disable();
            _playerControls.FindActionMap("UI").Enable();
        }
        else
        {
            _playerControls.FindActionMap("Gameplay").Enable();
            _playerControls.FindActionMap("UI").Disable();
        }
    }

    void Update()
    {
        Vector2 movement = _playerControls.FindActionMap("Gameplay").FindAction("Movement").ReadValue<Vector2>();

        _playerController.Move(movement);
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
    #endregion
}
