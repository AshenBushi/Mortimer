using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SkillRandomizer : MonoBehaviour
{
    private const int MaxLevel = 6;
    private const float DefaultRareChance = 20;
    private const float DefaultMythicalChance = 10;
    private const float DefaultLegendaryChance = 5;
    
    [Header("Skills Type")]
    [SerializeField] private List<SkillName> _commonSkills;
    [SerializeField] private List<SkillName> _rareSkills;
    [SerializeField] private List<SkillName> _mythicalSkills;
    [SerializeField] private List<SkillName> _legendarySkills;
    
    private SkillName RandomSkill(float commonChance, float rareChance, float mythicalChance)
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
    
    public List<SkillName> GetRandomSkills()
    {
        if(SkillsHandler.Instance.SkillsLevels.Count(level => level == MaxLevel) == 9) return null;

        var selectedSkills = new List<SkillName>();
        
        var currentRareChance = DefaultRareChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentMythicalChance = DefaultMythicalChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentLegendaryChance = DefaultLegendaryChance * PerksHandler.Instance.GetPerkBoost(PerkName.Luck);
        var currentCommonChance = 100f - currentRareChance - currentMythicalChance - currentLegendaryChance;

        for (var i = 0; i < 3; i++)
        {
            var skill = RandomSkill(currentCommonChance, currentRareChance, currentMythicalChance);

            while (SkillsHandler.Instance.SkillsLevels[(int)skill] == MaxLevel)
            {
                skill = RandomSkill(currentCommonChance, currentRareChance, currentMythicalChance);
            }
            
            selectedSkills.Add(skill);
        }

        return selectedSkills;
    }

    public SkillRarity GetSkillRarity(SkillName skillName)
    {
        if (_commonSkills.Contains(skillName)) return SkillRarity.Common;
        
        if (_rareSkills.Contains(skillName)) return SkillRarity.Rare;
        
        return _mythicalSkills.Contains(skillName) ? SkillRarity.Mythical : SkillRarity.Legendary;
    }
}
