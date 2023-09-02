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
