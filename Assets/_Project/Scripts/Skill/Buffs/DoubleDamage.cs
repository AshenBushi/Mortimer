using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : Buff
{
    public bool IsDoubleDamage { get; private set; } = false;

    protected override void Deactivate()
    {
        base.Deactivate();
        IsDoubleDamage = false;
    }

    public override void Activate()
    {
        base.Activate();
        IsDoubleDamage = true;
    }
}
