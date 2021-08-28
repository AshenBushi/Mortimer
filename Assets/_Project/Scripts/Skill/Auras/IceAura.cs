using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAura : Aura
{
    [SerializeField] private float _freezePower;
    [SerializeField] private float _delay;

    private float _timeSpend;
    
    private void Update()
    {
        _timeSpend += Time.deltaTime;

        if (!(_timeSpend >= _delay)) return;
        Freeze();
        _timeSpend = 0f;
    }

    private void Freeze()
    {
        CheckEnemyList();
        
        foreach (var enemy in EnemiesInZone)
        {
            enemy.SetAttackSpeed(1f + _freezePower);
        }
    }

    public void SetFreezePower(float value)
    {
        _freezePower = value;
    }
}
