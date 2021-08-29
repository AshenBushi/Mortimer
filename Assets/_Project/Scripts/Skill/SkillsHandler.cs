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
    [Header("Active Skills")]
    [SerializeField] private List<ActiveSkill> _activeSkills;
    [SerializeField] private DoubleDamage _doubleDamage;
    [SerializeField] private UltimateDefense _ultimateDefense;
    [Header("Auras")]
    [SerializeField] private FireAura _fireAura;
    [SerializeField] private IceAura _iceAura;
    [Header("Lists")]
    [SerializeField] private List<int> _skillsLevels;
    [SerializeField] private List<SkillBuffs> _skillsBuffs;
    [Header("Skills Type")]
    [SerializeField] private List<SkillName> _commonSkills;
    [SerializeField] private List<SkillName> _rareSkills;
    [SerializeField] private List<SkillName> _mythicalSkills;
    [SerializeField] private List<SkillName> _legendarySkills;
    


    private int _currentLevel = 1;
    
    public int ExperienceToLevel { get; private set; }
    public int CurrentExperience { get; private set; } = 0;
    public List<int> SkillsLevels => _skillsLevels;
    public event UnityAction OnExperienceChanged;

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
        
        _skillPanel.Show();

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
                if(!_activeSkills[0].IsActive)
                    _activeSkills[0].Enable();
                
                _activeSkills[0].SetCooldown((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.Dodge:
                _player.SetDodgeChance((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.DoubleDamage:
                if(!_activeSkills[2].IsActive)
                    _activeSkills[2].Enable();
                
                _doubleDamage.SetDuration(_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.UltimateShield:
                if(!_activeSkills[1].IsActive)
                    _activeSkills[1].Enable();
                
                _ultimateDefense.SetDuration(_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.FireAura:
                if(!_fireAura.gameObject.activeSelf)
                    _fireAura.gameObject.SetActive(true);
                
                _fireAura.SetDamage((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.FreezingSkin:
                if(!_iceAura.gameObject.activeSelf)
                    _iceAura.gameObject.SetActive(true);
                
                _iceAura.SetFreezePower(_skillsBuffs[(int)skillName].Buffs[level]);
                break;
        }
        
        _skillPanel.Hide();
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
    DoubleDamage,
    UltimateShield,
    FireAura,
    FreezingSkin
}

[Serializable]
public struct SkillBuffs
{
    public List<float> Buffs;
}

