using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerksHandler : Singleton<PerksHandler>
{
    [SerializeField] private List<Perk> _perks;

    private void Start()
    {
        StartCoroutine(InitPerks());
    }

    private IEnumerator InitPerks()
    {
        yield return new WaitForSeconds(0f);
        
        for (var i = 0; i < _perks.Count; i++)
        {
            _perks[i].Init(DataManager.Instance.Data.PerksLevels[i]);
        }
    }

    public void SavePerks()
    {
        for (var i = 0; i < _perks.Count; i++)
        {
            DataManager.Instance.Data.PerksLevels[i] = _perks[i].CurrentLevel;
        }
    }
    
    public float GetPerkBoost(PerkName name)
    {
        return _perks[(int)name].CurrentBoost;
    }
}

public enum PerkName
{
    Health,
    Damage,
    Veteran,
    Luck,
    Income
}