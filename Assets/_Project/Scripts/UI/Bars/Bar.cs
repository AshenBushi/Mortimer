using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Bar : MonoBehaviour
{
    [SerializeField] private float _changeSpeed = 10;
    
    private Slider _bar;
    private int _currentValue;

    private void Awake()
    {
        _bar = GetComponent<Slider>();
    }
    
    private IEnumerator UpdateBar()
    {
        while (_bar.value != _currentValue)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, _currentValue, _changeSpeed * Time.deltaTime);

            yield return null;
        }
    }

    protected void ChangeBarValue(int value)
    {
        _currentValue = value;

        StartCoroutine(UpdateBar());
    }

    protected virtual void SetBarValue(int maxValue, int currentValue)
    {
        _currentValue = currentValue;
        _bar.maxValue = maxValue;
        _bar.value = currentValue;
    }
}
