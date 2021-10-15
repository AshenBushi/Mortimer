using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SkillsHandler : Singleton<SkillsHandler>
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerStateHandler _playerStateHandler;
    [SerializeField] private ExperienceBar _experienceBar;
    [SerializeField] private SkillPanel _skillPanel;
    [SerializeField] private SkillRandomizer _skillRandomizer;
    [Header("Lists")]
    [SerializeField] private List<int> _skillsLevels;
    [SerializeField] private List<SkillBuffs> _skillsBuffs;
    
    private int _currentLevel = 1;
    public List<int> SkillsLevels => _skillsLevels;
    public event UnityAction OnExperienceChanged;

    public void Init()
    {
        for (var i = 0; i < _skillsLevels.Count; i++)
        {
            _skillsLevels[i] = 0;
        }
    }

    private void OnEnable()
    {
        _experienceBar.OnLevelUpgrade += GiveRandomSkills;
    }

    private void OnDisable()
    {
        _experienceBar.OnLevelUpgrade -= GiveRandomSkills;
    }

    public void GiveRandomSkills()
    {
        var selectedSkills = _skillRandomizer.GetRandomSkills();

        if (selectedSkills == null) return;
        
        _skillPanel.Show(AnimationName.Instantly);

        for (var i = 0; i < selectedSkills.Count; i++)
        {
            _skillPanel.SkillUis[i].Init(selectedSkills[i], _skillRandomizer.GetSkillRarity(selectedSkills[i]), _skillsBuffs[(int)selectedSkills[i]].Buffs[_skillsLevels[(int)selectedSkills[i]]]);
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
                break;
            case SkillName.StonePeaks:
                _player.UpgradeStonePeaks((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.Dodge:
                _player.SetDodgeChance((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.DoubleDamage:
                _player.UpgradeDoubleDamage((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.UltimateShield:
                _player.UpgradeUltimateDefense((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.FireAura:
                _player.UpgradeFireAura((int)_skillsBuffs[(int)skillName].Buffs[level]);
                break;
            case SkillName.FreezingSkin:
                _player.UpgradeIceAura(_skillsBuffs[(int)skillName].Buffs[level]);
                break;
        }
        
        FirebaseAnalytics.LogEvent($"skill_upgraded_{skillName}");
        
        _skillPanel.Hide(AnimationName.Instantly);
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
public enum SkillRarity
{
    Common,
    Rare,
    Mythical,
    Legendary
}

[Serializable]
public struct SkillBuffs
{
    public List<float> Buffs;
}

