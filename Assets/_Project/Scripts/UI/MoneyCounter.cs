using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private TMP_Text _text;
    private int _moneyCount = -1;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        _player.OnMoneyChanged += UpdateCounter;
    }

    private void OnDisable()
    {
        _player.OnMoneyChanged -= UpdateCounter;
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        _moneyCount = _player.Money;
        _text.text = _moneyCount.ToString();
    }
}
