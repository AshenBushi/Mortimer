using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillsHandler : Singleton<SkillsHandler>
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private ExperienceBar _experienceBar;

    private int _currentLevel = 1;
    
    public int ExperienceToLevel { get; private set; }
    public int CurrentExperience { get; private set; } = 0;
    public event UnityAction OnExperienceChanged;
    
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        UpdateBar();
    }
    
    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        CurrentExperience += (int)(enemy.ExperienceReward * PerksHandler.Instance.GetPerkBoost(PerksName.Veteran));

        if (CurrentExperience >= ExperienceToLevel)
        {
            _currentLevel++;
            CurrentExperience = 0;
            UpdateBar();
            GiveRandomSkills();
        }
        
        OnExperienceChanged?.Invoke();
    }

    private void UpdateBar()
    {
        ExperienceToLevel = _currentLevel * 100;
        _experienceBar.SetBarValue(ExperienceToLevel, CurrentExperience);
    }

    private void GiveRandomSkills()
    {
        Debug.Log("New Skill!");
    }
}

