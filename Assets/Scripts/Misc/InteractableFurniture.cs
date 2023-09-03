using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableFurniture : MonoBehaviour
{
    [SerializeField] private InputActionAsset _playerControls;
    [SerializeField] private SpriteRenderer _furniture;
    [SerializeField] private Sprite _furniture_closed;
    [SerializeField] private Sprite _furniture_open;
    private Animator _anim;
    private AudioSource _source;

    private bool _isLooted;
    private bool _isClose;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();

        _playerControls.FindActionMap("Gameplay").FindAction("Interact").performed += Interact; ;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (_isClose && !_isLooted)
        {
            _isLooted = true;

            PlayerController.Singleton.AddMoney(Random.Range(10, 50));
            _furniture.sprite = _furniture_open;

            PlayerController.Singleton.DiplayBalance(2);
            _source.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.tag == "Player")
        {
            if (!_isLooted)
            {
                _anim.SetBool("Display", true);
            }

            _isClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.tag == "Player")
        {
            _anim.SetBool("Display", false);
        }

        _isClose = false;
    }

    private void OnDisable()
    {
        _isLooted = false;

        _furniture.sprite = _furniture_closed;
    }
}
