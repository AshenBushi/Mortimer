using TMPro;
using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private Player _target;
    [SerializeField] private TMP_Text _text;

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
        SetBarValue(_target.PlayerStats.MaxHealth, _target.PlayerStats.Health);
    }

    /*protected override void ChangeBarValue(int value)
    {
        base.ChangeBarValue(value);
        _text.text = value.ToString();
    }*/

    protected override void SetBarValue(int maxValue, int currentValue)
    {
        base.SetBarValue(maxValue, currentValue);
        _text.text = currentValue.ToString();
    }
}
