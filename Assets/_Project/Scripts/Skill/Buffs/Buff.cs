using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] private GameObject _buffVFX;
    [SerializeField] private float _duration;

    private float _timeSpend;
    private bool _isActive = false;

    private void Update()
    {
        Countdown();
    }

    private void Countdown()
    {
        if(!_isActive) return;

        if (_timeSpend >= _duration)
        {
            Deactivate();
        }
        
        _timeSpend += Time.deltaTime;
    }

    protected virtual void Deactivate()
    {
        _isActive = false;
        _buffVFX.SetActive(false);
    }
    
    public void SetDuration(float value)
    {
        _duration = value;
    }

    public virtual void Activate()
    {
        _isActive = true;
        _timeSpend = 0;
        _buffVFX.SetActive(true);
    }
}
