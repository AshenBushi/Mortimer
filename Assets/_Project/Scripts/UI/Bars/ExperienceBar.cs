using UnityEngine;

public class ExperienceBar : Bar
{
    [SerializeField] private SkillsHandler _skillsHandler;

    private void OnEnable()
    {
        _skillsHandler.OnExperienceChanged += OnExperienceChanged;
    }
    
    private void OnDisable()
    {
        _skillsHandler.OnExperienceChanged -= OnExperienceChanged;
    }

    private void OnExperienceChanged()
    {
        ChangeBarValue(_skillsHandler.CurrentExperience);
    }
}
