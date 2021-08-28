using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateShield : Buff
{
    public bool IsUltimateShield { get; private set; } = false;

    protected override void Deactivate()
    {
        base.Deactivate();
        IsUltimateShield = false;
    }

    public override void Activate()
    {
        base.Activate();
        IsUltimateShield = true;
    }
}
