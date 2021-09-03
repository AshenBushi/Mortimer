using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    private const int MaxLevel = 5;

    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selectEffect;
    [Header("Description")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private string _buffPrefix;
    [Header("Setup")]
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private List<int> _prices;
    [SerializeField] private List<float> _boosts;

    private Player _player;
    private PerkDescription _perkDescription;
    private int _currentLevel = 0;

    public float CurrentBoost => _boosts[_currentLevel];
    public int CurrentLevel => _currentLevel;

    public void Init(Player player, PerkDescription perkDescription, int level)
    {
        _currentLevel = level;
        _player = player;
        _perkDescription = perkDescription;
        SetIcon();
    }

    private void SetIcon()
    {
        _icon.sprite = _icons[_currentLevel];
        _icon.SetNativeSize();
        _icon.transform.position = transform.position;
    }

    public void ShowDescription()
    {
        var data = new PerkDescriptionData
        {
            Name = _name,
            Description = _description,
            CurrentLevel = _currentLevel,
            CurrentBuff = _buffPrefix + _boosts[_currentLevel],
            NextBuff = _buffPrefix + _boosts[_currentLevel + 1],
            Price = _prices[_currentLevel + 1]
        };

        _perkDescription.ShowDescription(data, this);
    }
    
    public void Upgrade()
    {
        _currentLevel++;

        _player.SpendMoney(_prices[_currentLevel]);

        SetIcon();
        ShowDescription();
        
        PerksHandler.Instance.SavePerks();
    }

    public void EnableSelectedEffect()
    {
        _selectEffect.SetActive(true);
    }
    
    public void DisableSelectedEffect()
    {
        _selectEffect.SetActive(false);
    }
}
