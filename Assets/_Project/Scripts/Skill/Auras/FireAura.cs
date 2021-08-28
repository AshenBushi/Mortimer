using System;
using System.Collections.Generic;
using UnityEngine;

public class FireAura : Aura
{
    [SerializeField] private int _damage;
    [SerializeField] private float _delay;
    
    private float _timeSpend;

    private void Update()
    {
        _timeSpend += Time.deltaTime;

        if (!(_timeSpend >= _delay)) return;
        
        Flame();
        _timeSpend = 0;
    }

    private void Flame()
    {
        CheckEnemyList();
        
        foreach (var enemy in EnemiesInZone)
        {
            enemy.TakeDamage(_damage);
        }
    }

    public void SetDamage(int value)
    {
        _damage = value;
    }
}
