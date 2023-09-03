using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsVFX : MonoBehaviour
{
    [SerializeField] private Image _coinTemplate;

    private Image[] _coins;
    private Vector2[] _coinsDirections;

    private float _damp;
    private float _dampReduction;
    private float _speed;
    private Vector2 _dest;

    #region Unity
    private void Update()
    {
        if (_damp < 0.1f)
        {
            bool allReady = true;

            for (int i = 0; i < _coins.Length; i++)
            {
                var position = _coins[i].rectTransform.position;
                var targetPos = _dest;

                _speed += Time.deltaTime * 35;

                _coins[i].rectTransform.position = Vector2.MoveTowards(position, targetPos, Time.deltaTime * _speed * 10);

                if (Vector2.Distance(position, targetPos) > 1)
                    allReady = false;
            }

            if(allReady)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _damp = Mathf.Lerp(_damp, 0, Time.deltaTime * _dampReduction);

            for (int i = 0; i < _coins.Length; i++)
            {
                var position = _coins[i].rectTransform.position;
                var targetPos = new Vector2(position.x + _coinsDirections[i].x, position.y + _coinsDirections[i].y);

                _coins[i].rectTransform.position = Vector2.Lerp(position, targetPos, Time.deltaTime * _damp);
            }
        }
    }
    #endregion

    #region Public
    public void Init(int coinsAmount, float initialDamp, float speed, Vector2 origin, Vector2 dest)
    {
        _damp = initialDamp;
        _dampReduction = _damp;
        _speed = speed;
        _dest = dest;

        _coins = new Image[coinsAmount];
        _coinsDirections = new Vector2[coinsAmount];

        for (int i = 0; i < coinsAmount; i++)
        {
            _coins[i] = Instantiate(_coinTemplate, transform);
            _coins[i].gameObject.SetActive(true);
            _coins[i].rectTransform.position = origin;

            _coinsDirections[i] = origin * Random.insideUnitCircle * 0.1f;
        }
    }
    #endregion
}
