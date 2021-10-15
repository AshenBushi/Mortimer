using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public const int MaxLevel = 15;

    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selectEffect;
    [SerializeField] private List<LevelIndicator> _levelIndicators;
    [Header("Description")]
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private string _buffPrefix;
    [Header("Setup")] 
    [SerializeField] private Sprite _defaultIcon;
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
        switch (_currentLevel)
        {
            case MaxLevel:
            {
                _icon.sprite = _icons[_icons.Count - 1];
                
                foreach (var indicator in _levelIndicators)
                {
                    indicator.Hide();
                }

                break;
            }
            default:
            {
                _icon.sprite = _icons[_currentLevel / 3];
                
                for (var i = 0; i < _levelIndicators.Count; i++)
                {
                    _levelIndicators[i].Show();
            
                    if (_currentLevel % 3 >= i)
                    {
                        _levelIndicators[i].Enable();
                    }
                    else
                    {
                        _levelIndicators[i].Disable();
                    }
                }

                break;
            }
        }
        
        _icon.SetNativeSize();
    }

    public void ShowDescription()
    {
        if (_currentLevel < MaxLevel)
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
        else
        {
            var data = new PerkDescriptionData
            {
                Name = _name,
                Description = _description,
                CurrentLevel = _currentLevel,
                CurrentBuff = _buffPrefix + _boosts[_currentLevel],
                NextBuff = _buffPrefix + _boosts[_currentLevel],
                Price = _prices[_currentLevel]
            };

            _perkDescription.ShowMaxLevelDescription(data, this);
        }
    }
    
    public void Upgrade()
    {
        if(!Wallet.Instance.TrySpendMoney(_prices[_currentLevel])) return;
        
        _currentLevel++;

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
