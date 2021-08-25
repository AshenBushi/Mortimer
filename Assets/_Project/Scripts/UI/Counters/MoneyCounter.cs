using System;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        User.Instance.OnMoneyChanged += UpdateCounter;
    }
    
    private void OnDisable()
    {
        User.Instance.OnMoneyChanged += UpdateCounter;
    }

    private void UpdateCounter()
    {
        _text.text = User.Instance.Money.ToString();
    }
}
