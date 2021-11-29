using System;
using UnityEngine;

public class StaminaBar : Bar
{
    [SerializeField] private Stamina _stamina;

    private void OnEnable()
    {
        _stamina.OnStaminaChanged += OnStaminaChanged;
    }

    private void OnDisable()
    {
        _stamina.OnStaminaChanged -= OnStaminaChanged;
    }

    private void Start()
    {
        SetBarValue(_stamina.MaxStaminaCount, _stamina.MaxStaminaCount);
    }

    private void OnStaminaChanged()
    {
        ChangeBarValue(_stamina.StaminaCount);
    }
}
