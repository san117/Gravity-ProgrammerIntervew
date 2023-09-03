using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [Header("Settings")]
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _damp = 1;
    [SerializeField] private int _money = 10;

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private Animator _balanceView;
    [SerializeField] private InventorySlotView _headEquipmentSlot;
    [SerializeField] private InventorySlotView _chestEquipmentSlot;
    [SerializeField] private InventoryModel _playerEquipment;

    private ItemModel _headEquipment;
    private ItemModel _chestEquipment;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    private float _displayingBalanceTimer;

    public int Balance => _money;
    #endregion

    private static PlayerController _singleton;
    public static PlayerController Singleton
    {
        get
        {
            if (_singleton == null)
                _singleton = FindObjectOfType<PlayerController>();

            return _singleton;
        }
    }

    private Vector2 _targetVelocity;
    private Vector2 _animatorSpeed;

    #region Unity
    private void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();

        _headEquipmentSlot._onSetItemEvent += EquipHead;
        _chestEquipmentSlot._onSetItemEvent += EquipChest;

        EquipHead(_playerEquipment.inventory[0]);
        EquipChest(_playerEquipment.inventory[1]);

        DiplayBalance(3);
    }

    private void Update()
    {
        var velocity = Vector2.Lerp(_rb2d.velocity, _targetVelocity, Time.deltaTime * _damp);

        _rb2d.velocity = velocity;

        if(_displayingBalanceTimer > 0)
        {
            _displayingBalanceTimer = Mathf.MoveTowards(_displayingBalanceTimer, 0, Time.deltaTime);

            if (_displayingBalanceTimer == 0)
            {
                _balanceView.SetBool("Display", false);
            }
        }

        AnimationController();
    }
    #endregion

    #region Public
    public void Move(Vector2 axis)
    {
        _targetVelocity = axis * _speed;
    }

    public void AddMoney(int amount)
    {
        _money += Mathf.Abs(amount);
    }

    public void SetMoney(int amount)
    {
        _money = amount;
    }

    public void SubstractMoney(int amount)
    {
        _money -= Mathf.Abs(amount);
    }

    public bool HasEnoughMoney(int amount)
    {
        return _money >= amount;
    }

    public void EquipHead(ItemModel model)
    {
        if (_headEquipment != null)
            _animator.SetLayerWeight(_headEquipment.EquipLayerMask, 0);

        _headEquipment = model;

        if (_headEquipment != null)
            _animator.SetLayerWeight(_headEquipment.EquipLayerMask, 1);
    }

    public void EquipChest(ItemModel model)
    {
        if (_chestEquipment != null)
            _animator.SetLayerWeight(_chestEquipment.EquipLayerMask, 0);

        _chestEquipment = model;

        if (_chestEquipment != null)
            _animator.SetLayerWeight(_chestEquipment.EquipLayerMask, 1);
    }

    public void DiplayBalance(float time)
    {
        if(_money >= 0)
            _balanceText.text = "$" + _money;
        else
            _balanceText.text = "<color=#FF5E00>$" + _money + "</color>";

        _balanceView.SetBool("Display", true);

        _displayingBalanceTimer = time;
    }
    #endregion

    #region Internal
    private void AnimationController()
    {
        var targetAnimatorSpeed = _rb2d.velocity;

        if (targetAnimatorSpeed.magnitude > 0.3f)
            _animatorSpeed = targetAnimatorSpeed;
        else if (_animatorSpeed.magnitude > 0.3f)
            _animatorSpeed = Vector2.MoveTowards(_animatorSpeed, Vector2.zero, Time.deltaTime * 5);

        _animator.SetFloat("Horizontal Speed", _animatorSpeed.x);
        _animator.SetFloat("Vertical Speed", _animatorSpeed.y);
    }
    #endregion
}
