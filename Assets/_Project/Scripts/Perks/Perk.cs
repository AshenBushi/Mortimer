using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    private const int MaxLevel = 5; 
    
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private List<int> _prices;
    [SerializeField] private List<float> _boosts;
    
    private Image _icon;
    private Button _button;
    private Animator _animator;
    private int _currentLevel = 0;
    private bool _isMaxLevel = false;

    public float CurrentBoost => _boosts[_currentLevel];
    public int CurrentLevel => _currentLevel;

    private void Awake()
    {
        _icon = GetComponent<Image>();
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
    }

    public void Init(int level)
    {
        _currentLevel = level;
        SetIcon();
        CheckSolvency();
        User.Instance.OnMoneyChanged += CheckSolvency;
    }

    private void OnEnable()
    {
        if(User.Instance != null)
            User.Instance.OnMoneyChanged += CheckSolvency;
    }

    private void OnDisable()
    {
        User.Instance.OnMoneyChanged -= CheckSolvency;
    }

    private void CheckSolvency()
    {
        if(!gameObject.activeSelf) return;
        
        if (_currentLevel == MaxLevel)
            _isMaxLevel = true;

        if (_isMaxLevel)
        {
            _button.interactable = false;
            _animator.Play("Idle");
            return;
        }

        if (User.Instance.Money >= _prices[_currentLevel + 1])
        {
            _animator.Play("PerkEnable");
            _button.interactable = true;
        }
        else
        {
            _animator.Play("Idle");
            _button.interactable = false;
        }
    }

    private void SetIcon()
    {
        _icon.sprite = _icons[_currentLevel];
        _icon.SetNativeSize();
    }
    
    public void Upgrade()
    {
        _currentLevel++;

        User.Instance.SpendMoney(_prices[_currentLevel]);

        SetIcon();
        
        PerksHandler.Instance.SavePerks();
    }
}
