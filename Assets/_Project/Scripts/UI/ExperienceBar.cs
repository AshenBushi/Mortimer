using UnityEngine;

public class ExperienceBar : Bar
{
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.OnExperienceChanged += OnExperienceChanged;
    }
    
    private void OnDisable()
    {
        _player.OnExperienceChanged -= OnExperienceChanged;
    }

    private void Start()
    {
        SetBarValue(100, _player.PlayerStats.Experience);
    }

    private void OnExperienceChanged()
    {
        ChangeBarValue(_player.PlayerStats.Experience);
    }
}
