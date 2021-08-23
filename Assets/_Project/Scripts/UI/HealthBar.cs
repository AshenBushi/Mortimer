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
        SetBarValue(_target.MaxHealth, _target.Health);
    }

    private void OnHealthChanged()
    {
        StartCoroutine(ChangeBarValue(_target.Health));
    }
}
