using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkDescription : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Header("Setup")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _currentLevel;
    [SerializeField] private TMP_Text _nextLevel;
    [SerializeField] private TMP_Text _currentBuff;
    [SerializeField] private TMP_Text _nextBuff;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Button _buyButton;

    private Perk _currentPerk;
    private PerkDescriptionData _currentData;

    private void OnEnable()
    {
        _player.OnMoneyChanged += CheckSolvency;
    }

    private void OnDisable()
    {
        _player.OnMoneyChanged -= CheckSolvency;
    }

    private void CheckSolvency()
    {
        _buyButton.interactable = _player.Money >= _currentData.Price;
    }
    
    public void ShowDescription(PerkDescriptionData data, Perk currentPerk)
    {
        _name.text = data.Name;
        _description.text = data.Description;
        _currentLevel.text = $"Current Level: {data.CurrentLevel}";
        _nextLevel.text = $"Next level: {data.CurrentLevel + 1}";
        _currentBuff.text = data.CurrentBuff;
        _nextBuff.text = data.NextBuff;
        _price.text = data.Price.ToString();

        _currentData = data;
        _currentPerk = currentPerk;
        
        CheckSolvency();
        
        PerksHandler.Instance.SelectPerk(_currentPerk);
    }

    public void UpgradePerk()
    {
        _currentPerk.Upgrade();
    }
}

[Serializable]
public struct PerkDescriptionData
{
    public string Name;
    public string Description;
    public int CurrentLevel;
    public string CurrentBuff;
    public string NextBuff;
    public int Price;
}