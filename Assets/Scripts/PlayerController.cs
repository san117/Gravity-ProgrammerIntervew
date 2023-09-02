using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _damp = 1;

    private Rigidbody2D _rb2d;
    private Animator _animator;
    #endregion

    private Vector2 _targetVelocity;
    private Vector2 _animatorSpeed;

    #region Unity
    private void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        var velocity = Vector2.Lerp(_rb2d.velocity, _targetVelocity, Time.deltaTime * _damp);

        _rb2d.velocity = velocity;

        AnimationController();
    }
    #endregion

    #region Public
    public void Move(Vector2 axis)
    {
        _targetVelocity = axis * _speed;
    }
    #endregion

    #region Internal
    private void AnimationController()
    {
        var targetAnimatorSpeed = _rb2d.velocity;

        if (targetAnimatorSpeed.magnitude > 0.3f)
            _animatorSpeed = targetAnimatorSpeed;
        else if (_animatorSpeed.magnitude > 0.1f)
            _animatorSpeed = Vector2.MoveTowards(_animatorSpeed, Vector2.zero, Time.deltaTime * 5);

        _animator.SetFloat("Horizontal Speed", _animatorSpeed.x);
        _animator.SetFloat("Vertical Speed", _animatorSpeed.y);
    }
    #endregion
}
