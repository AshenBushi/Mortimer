using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkDescription : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Header("Setup")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private GameObject _levelsAndBuffs;
    [SerializeField] private GameObject _maxLevel;
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
        Wallet.Instance.OnMoneyChanged += CheckSolvency;
    }

    private void OnDisable()
    {
        Wallet.Instance.OnMoneyChanged -= CheckSolvency;
    }

    private void CheckSolvency()
    {
        _buyButton.interactable = Wallet.Instance.Money >= _currentData.Price;
    }
    
    public void ShowDescription(PerkDescriptionData data, Perk currentPerk)
    {
        _levelsAndBuffs.SetActive(true);
        _maxLevel.SetActive(false);
        _name.text = data.Name;
        _description.text = data.Description;
        _currentLevel.text = $"Current Level: {data.CurrentLevel}";
        _nextLevel.text = $"Next level: {data.CurrentLevel + 1}";
        _currentBuff.text = data.CurrentBuff;
        _nextBuff.text = data.NextBuff;
        _price.text = data.Price.ToString();

        _currentData = data;
        _currentPerk = currentPerk;
        
        _buyButton.gameObject.SetActive(true);
        CheckSolvency();
        
        PerksHandler.Instance.SelectPerk(_currentPerk);
    }

    public void ShowMaxLevelDescription(PerkDescriptionData data, Perk currentPerk)
    {
        _levelsAndBuffs.SetActive(false);
        _maxLevel.SetActive(true);
        _name.text = data.Name;
        _description.text = data.Description;
        
        _currentPerk = currentPerk;
        
        _buyButton.gameObject.SetActive(false);

        PerksHandler.Instance.SelectPerk(_currentPerk);
    }

    public void UpgradePerk()
    {
        _currentPerk.Upgrade();
        FirebaseAnalytics.LogEvent($"perk_upgraded_{_currentData.Name}");
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