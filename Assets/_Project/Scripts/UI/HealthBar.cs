using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private Player _target;

    private void OnEnable()
    {
        _target.OnHealthChanged += OnHealthChanged;
    }
    
    private void OnDisable()
    {
        _target.OnHealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        SetBarValue(_target.PlayerStats.MaxHealth, _target.PlayerStats.Health);
    }

    private void OnHealthChanged()
    {
        ChangeBarValue(_target.PlayerStats.Health);
    }
}
