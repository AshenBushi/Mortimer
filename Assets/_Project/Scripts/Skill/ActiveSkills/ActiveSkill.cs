using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private Image _enableIcon;
    [SerializeField] private GameObject _outLine;

    private Button _button;
    private bool _isActive = false;
    private float _timeSpend = 0f;

    public bool IsActive => _isActive;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Init()
    {
        _isActive = false;
        _button.interactable = false;
        _outLine.SetActive(false);
        _timeSpend = 0;
        _enableIcon.fillAmount = 0f;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Cooldown();
    }

    public void Enable()
    {
        _isActive = true;
        _button.interactable = true;
        _outLine.SetActive(true);
        _enableIcon.fillAmount = 1f;
    }

    private void Cooldown()
    {
        if(!_isActive) return;
        if (_button.interactable) return;

        var fill = _timeSpend / _cooldown;
        
        _enableIcon.fillAmount = fill > 1 ? 1f : fill;
        
        if (_timeSpend >= _cooldown)
        {
            Enable();
        }

        _timeSpend += Time.deltaTime;
    }

    public void UseSkill()
    {
        _button.interactable = false;
        _enableIcon.fillAmount = 0f;
        _outLine.SetActive(false);
        _timeSpend = 0f;
    }
    
    public void SetCooldown(int value)
    {
        _cooldown = value;
    }
}
