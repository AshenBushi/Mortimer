using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksHandler : Singleton<PerksHandler>
{
    [SerializeField] private GameObject _perksContainer;
    [SerializeField] private Player _player;
    [SerializeField] private PerkDescription _perkDescription;

    private List<Perk> _perks = new List<Perk>();

    protected override void Awake()
    {
        base.Awake();

        _perks = _perksContainer.GetComponentsInChildren<Perk>().ToList();
    }

    private void Start()
    {
        InitPerks();
    }

    private void InitPerks()
    {
        for (var i = 0; i < _perks.Count; i++)
        {
            _perks[i].Init(_player, _perkDescription, DataManager.Instance.Data.PerksLevels[i]);
        }
        
        _perks[0].ShowDescription();
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

    public void SelectPerk(Perk selectedPerk)
    {
        foreach (var perk in _perks)
        {
            if(perk == selectedPerk)
                perk.EnableSelectedEffect();
            else
                perk.DisableSelectedEffect();
        }
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