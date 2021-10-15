using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ExperienceBar : Bar
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TMP_Text _level;

    private int _experienceToLevel;
    private int _currentExperience = 0;
    private int _currentLevel = 1;
    public event UnityAction OnLevelUpgrade;
    
    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += OnEnemyKilled;
    }
    
    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= OnEnemyKilled;
    }

    private void Start()
    {
        UpdateBar();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        _currentExperience += (int)(enemy.ExperienceReward * PerksHandler.Instance.GetPerkBoost(PerkName.Veteran));

        if (_currentExperience >= _experienceToLevel)
        {
            _currentLevel++;
            _currentExperience = 0;
            UpdateBar();
            OnLevelUpgrade?.Invoke();
        }
        
        ChangeBarValue(_currentExperience);
    }

    private void UpdateBar()
    {
        _experienceToLevel = _currentLevel * 100;
        _level.text = _currentLevel.ToString();
        SetBarValue(_experienceToLevel, _currentExperience);
    }
}
