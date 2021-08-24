using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _text;
   
    private int _moneyCount;

    public void UpdateCounter()
    {
        _moneyCount = _player.Money;
        _text.text = _moneyCount.ToString();
    }
}
