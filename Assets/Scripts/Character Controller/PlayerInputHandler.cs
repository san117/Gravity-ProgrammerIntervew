using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Fields
    [SerializeField] private InputActionAsset _playerControls;
    [SerializeField] private PlayerController _playerController;
    #endregion

    #region Unity
    void Start()
    {
        _playerControls.FindActionMap("Gameplay").FindAction("Inventory").performed += InventoryButtonPressed;
        _playerControls.FindActionMap("UI").FindAction("Inventory").performed += InventoryButtonPressed;
    }

    private void InventoryButtonPressed(InputAction.CallbackContext obj)
    {
        var inventoryController = FindObjectOfType<InventoryController>(true);

        var isDisplaying = !inventoryController.IsDisplaying;

        inventoryController.SetDisplay(isDisplaying);

        if(isDisplaying)
            _playerControls.FindActionMap("Gameplay").Disable();
        else
            _playerControls.FindActionMap("Gameplay").Enable();
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
