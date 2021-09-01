using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _player.OnMoneyChanged += UpdateCounter;
    }
    
    private void OnDisable()
    {
        _player.OnMoneyChanged += UpdateCounter;
    }

    private void Start()
    {
        UpdateCounter();
    }

    private void UpdateCounter()
    {
        _text.text = _player.Money.ToString();
    }
}
