using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Bar : MonoBehaviour
{
    [SerializeField] private float _changeSpeed = 1;
    
    private Slider _bar;
    private int _currentValue;

    private void Awake()
    {
        _bar = GetComponent<Slider>();
    }
    
    private void Update()
    {
        if(_bar.value - _currentValue == 0) return;

        _bar.value = Mathf.MoveTowards(_bar.value, _currentValue, _changeSpeed * Time.deltaTime);
    }

    protected virtual void ChangeBarValue(int value)
    {
        _currentValue = value;
    }

    protected virtual void SetBarValue(int maxValue, int currentValue)
    {
        _currentValue = currentValue;
        _bar.maxValue = maxValue;
        _bar.value = currentValue;
    }
}
