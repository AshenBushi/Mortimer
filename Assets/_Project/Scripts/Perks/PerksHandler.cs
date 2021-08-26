using System;
using System.Collections.Generic;
using UnityEngine;

public class PerksHandler : Singleton<PerksHandler>
{
    [SerializeField] private List<Perk> _perks;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitPerks();
    }

    private void InitPerks()
    {
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
    
    public float GetPerkBoost(PerksName name)
    {
        return _perks[(int)name].CurrentBoost;
    }
}

public enum PerksName
{
    Health,
    Damage,
    Veteran,
    Luck,
    Income
}