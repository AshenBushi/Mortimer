using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SkillsHandler : Singleton<SkillsHandler>
{
    private const int MaxLevel = 6;
    private const float DefaultRareChance = 20;
    private const float DefaultMythicalChance = 10;
    private const float DefaultLegendaryChance = 5;
    
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerAttackHandler _playerAttackHandler;
    [SerializeField] private ExperienceBar _experienceBar;
    [SerializeField] private SkillPanel _skillPanel;
    [SerializeField] private List<int> _skillsLevels;
    [SerializeField] private List<SkillName> _commonSkills;
    [SerializeField] private List<SkillName> _rareSkills;
    [SerializeField] private List<SkillName> _mythicalSkills;
    [SerializeField] private List<SkillName> _legendarySkills;
    [SerializeField] private List<SkillBuffs> _skillsBuffs;


    private int _currentLevel = 1;
    
    public int ExperienceToLevel { get; private set; }
    public int CurrentExperience { get; private set; } = 0;
    public List<int> SkillsLevels => _skillsLevels;
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
        for (var i = 0; i < _skillsLevels.Count; i++)
        {
            _skillsLevels[i] = 0;
        }
        
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
        CurrentExperience += (int)(enemy.ExperienceReward * PerksHandler.Instance.GetPerkBoost(PerkName.Veteran));

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

    private SkillName GetRandomSkill(float commonChance, float rareChance, float mythicalChance, float legendaryChance)
    {
        var randomValue = Random.Range(1, 101);

        if (randomValue <= commonChance)
        {
            return _commonSkills[Random.Range(0, _commonSkills.Count)];
        }
        else
        {
            if (randomValue <= commonChance + rareChance)
            {
                return _rareSkills[Random.Range(0, _rareSkills.Count)];
            }
            else
            {
                return randomValue <= commonChance + rareChance + mythicalChance ? _mythicalSkills[Random.Range(0, _mythicalSkills.Count)] : _legendarySkills[Random.Range(0, _legendarySkills.Count)];
            }
        }
    }
    
    private void GiveRandomSkills()
    {
        if(_skillsLevels.Count(level => level == MaxLevel) == 9) return;

        var selectedSkills = new List<SkillName>();
        
        var currentRareChance = DefaultRareChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentMythicalChance = DefaultMythicalChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentLegendaryChance = DefaultLegendaryChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentCommonChance = 100f - currentRareChance - currentMythicalChance - currentLegendaryChance;

        for (var i = 0; i < 3; i++)
        {
            var skill = GetRandomSkill(currentCommonChance, currentRareChance, currentMythicalChance,
                currentLegendaryChance);

            while (_skillsLevels[(int)skill] == MaxLevel)
            {
                skill = GetRandomSkill(currentCommonChance, currentRareChance, currentMythicalChance,
                    currentLegendaryChance);
            }
            
            selectedSkills.Add(skill);
        }
        
        _skillPanel.Enable();

        for (var i = 0; i < selectedSkills.Count; i++)
        {
            _skillPanel.SkillUis[i].Init(selectedSkills[i]);
        }
    }

    public void GetSkill(SkillName skillName)
    {
        var level = _skillsLevels[(int)skillName]++;

        switch (skillName)
        {
            case SkillName.HPBoost:
                _player.IncreaseHealth((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.SwordMaster:
                _player.IncreaseDamage((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.AttackSpeed:
                _player.SetAttackSpeed(_skillsBuffs[(int)skillName].Buffs[level]);
                _playerAttackHandler.DecreaseCooldown(_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.StonePeaks:
                break;
            case SkillName.Dodge:
                _player.SetDodgeChance((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.Majesty:
                break;
            case SkillName.UltimateDefense:
                break;
            case SkillName.BlazingSkin:
                break;
            case SkillName.FreezingSkin:
                break;
        }
        
        _skillPanel.Disable();
    }
}


[Serializable]
public enum SkillName
{
    HPBoost,
    SwordMaster,
    AttackSpeed,
    StonePeaks,
    Dodge,
    Majesty,
    UltimateDefense,
    BlazingSkin,
    FreezingSkin
}

[Serializable]
public struct SkillBuffs
{
    public List<float> Buffs;
}

