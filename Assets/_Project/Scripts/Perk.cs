using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private int _price;
    
    public int CurrentLevel { get; private set; }

    private void OnEnable()
    {
        UpdateButtonStatus();
        UpdatePriceAndLevelText();
    }

    private void UpdateButtonStatus()
    {
        _buyButton.interactable = _player.Money >= _price;
    }

    private void UpdatePriceAndLevelText()
    {
        _priceText.text = "Level: " + _price;
        _levelText.text = CurrentLevel.ToString();
    }
    
    public void Upgrade()
    {
        CurrentLevel++;

        _player.SpendMoney(_price);

        _price *= 2;
        
        UpdatePriceAndLevelText();
        UpdateButtonStatus();
    }
}
