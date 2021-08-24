using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Bar : MonoBehaviour
{
    private Slider _bar;

    private void Awake()
    {
        _bar = GetComponent<Slider>();
    }

    protected void SetBarValue(int maxValue, int currentValue)
    {
        _bar.maxValue = maxValue;
        _bar.value = currentValue;
    }
    
    protected void ChangeBarValue(int value)
    {
        _bar.value = value;
    }
}
