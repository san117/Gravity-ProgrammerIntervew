using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MercancySlot : MonoBehaviour
{
    #region Fields
    [Header("Components")]
    [SerializeField] private Image _mercancyIcon;
    [SerializeField] private TextMeshProUGUI _mercancyTitle;
    [SerializeField] private TextMeshProUGUI _mercancyPrice;
    [SerializeField] private Button _button;

    private Shop.MercancyInfo _mercancyInfo;

    public Shop.MercancyInfo Mercancy => _mercancyInfo;
    #endregion

    #region Unity
    #endregion

    #region Public
    public void SetMercancy(Shop.MercancyInfo mercancyInfo, bool interactable)
    {
        _mercancyInfo = mercancyInfo;

        Paint(interactable);
    }

    public void SelectMercancy()
    {
        GetComponentInParent<Shop>().Buy(Mercancy);
    }
    #endregion

    #region Internal
    private void Paint(bool interactable)
    {
        if(_mercancyInfo != null)
        {
            gameObject.SetActive(true);

            _mercancyIcon.sprite = _mercancyInfo.item.Icon;
            _mercancyTitle.text = _mercancyInfo.item.Title;
            _mercancyPrice.text = "$" + _mercancyInfo.price;

            var priceColor = _mercancyPrice.color;

            priceColor.a = interactable? 1 : 0.5f;

            _mercancyPrice.color = priceColor;
            _button.interactable = interactable;
        }
        else
        {
            Destroy(gameObject);
        } 
    }
    #endregion
}
