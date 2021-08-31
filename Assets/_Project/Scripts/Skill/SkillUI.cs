using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private List<string> _rarities;
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private List<Sprite> _buttonBackgrounds;
    [SerializeField] private List<string> _descriptions;
    [Space]
    [SerializeField] private TMP_Text _rarity;
    [SerializeField] private TMP_Text _nextLevel;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _buttonBackground;
    [SerializeField] private TMP_Text _buff;
    [SerializeField] private TMP_Text _description;

    private SkillName _skillName;

    public void Init(SkillName skillName, SkillRarity rarity, float buff)
    {
        _skillName = skillName;
        _rarity.text = _rarities[(int)rarity];
        var nextLevel = SkillsHandler.Instance.SkillsLevels[(int)skillName] + 1;
        _nextLevel.text = nextLevel == 6 ? "Max": nextLevel.ToString();
        _icon.sprite = _icons[(int)skillName];
        _buttonBackground.sprite = _buttonBackgrounds[(int)rarity];

        _buff.text = skillName switch
        {
            SkillName.HPBoost => $"+{buff}",
            SkillName.SwordMaster => $"+{buff}",
            SkillName.AttackSpeed => $"{(buff - 1) * 100}%",
            SkillName.StonePeaks => $"Cooldown: {buff} sec",
            SkillName.Dodge => $"{buff}%",
            SkillName.DoubleDamage => $"Duration: {buff} sec",
            SkillName.UltimateShield => $"Duration: {buff} sec",
            SkillName.FireAura => $"{buff} per sec",
            SkillName.FreezingSkin => $"*{buff}",
            _ => _buff.text
        };

        _description.text = _descriptions[(int)skillName];
    }

    public void SelectSkill()
    {
        SkillsHandler.Instance.GetSkill(_skillName);
    }
}
