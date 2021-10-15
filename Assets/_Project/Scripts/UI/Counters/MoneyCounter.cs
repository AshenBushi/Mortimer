using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private int _currentMoneyCount;

    public int Money => Wallet.Instance.Money;
        
    private void OnEnable()
    {
        Wallet.Instance.OnMoneyChanged += UpdateCounter;
    }
    
    private void OnDisable()
    {
        Wallet.Instance.OnMoneyChanged += UpdateCounter;
    }

    private void Start()
    {
        SetCounter();
    }

    private void SetCounter()
    {
        _text.text = Money.ToString();
        _currentMoneyCount = Money;
    }
    
    private void UpdateCounter()
    {
        StartCoroutine(UpdateCounterCoroutine());
    }

    private IEnumerator UpdateCounterCoroutine()
    {
        Debug.Log("Count");
        
        while (Money != _currentMoneyCount)
        {
            _currentMoneyCount += Money > _currentMoneyCount ? 1 : -1;
            _text.text = _currentMoneyCount.ToString();

            yield return null;
        }
    }
}
