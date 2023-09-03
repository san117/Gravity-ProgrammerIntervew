using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class MercancyInfo
    {
        public int price;
        public ItemModel item;
    }

    #region Fields
    [Header("Settings")]
    [SerializeField] private MercancyInfo[] _aviableMercancy;

    [Header("Components")]
    [SerializeField] private MercancySlot _mercancyTemplate;
    [SerializeField] private Transform _content;
    [SerializeField] private TextMeshProUGUI _npcDialogue;
    #endregion

    #region Unity
    private void OnEnable()
    {
        UpdateMercancyView();
    }

    private void Update()
    {
        var color = _npcDialogue.color;

        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime);

        _npcDialogue.color = color;
    }
    #endregion

    #region Public
    public void Buy(MercancyInfo mercancyInfo)
    {
        _npcDialogue.text = "Thank you! It's one of my best works";

        var color = _npcDialogue.color;
        color.a = 1;
        _npcDialogue.color = color;
    }
    #endregion

    #region Internal
    private void UpdateMercancyView()
    {
        foreach (Transform child in _content)
        {
            if (child.gameObject != _mercancyTemplate.gameObject)
                Destroy(_mercancyTemplate.gameObject);
        }

        foreach (var mercancy in _aviableMercancy)
        {
            var slot = Instantiate(_mercancyTemplate, _content);
            slot.SetMercancy(mercancy, true);
        }
    }
    #endregion
}
