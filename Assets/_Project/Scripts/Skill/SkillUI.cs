using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private List<string> _names;
    [SerializeField] private List<Sprite> _icons;
    [SerializeField] private List<Sprite> _buttonBackgrounds;
    [Space]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _buttonBackground;

    private SkillName _skillName;

    public void Init(SkillName skillName)
    {
        _skillName = skillName;
        _name.text = _names[(int)skillName];
        _icon.sprite = _icons[(int)skillName];
        _buttonBackground.sprite = _buttonBackgrounds[SkillsHandler.Instance.SkillsLevels[(int)_skillName]];
    }

    public void SelectSkill()
    {
        SkillsHandler.Instance.GetSkill(_skillName);
    }
}
