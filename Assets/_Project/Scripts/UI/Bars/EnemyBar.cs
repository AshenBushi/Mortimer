using System;
using UnityEngine;

public class EnemyBar : Bar
{
    public void SetValue(int maxValue, int value)
    {
        SetBarValue(maxValue, value);
    }

    public void ChangeValue(int value)
    {
        ChangeBarValue(value);
    }
}
