using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillPanel : UIPanel
{
    public List<SkillUI> SkillUis { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        SkillUis = GetComponentsInChildren<SkillUI>().ToList();
    }

    public override void Show()
    {
        base.Show();
        Time.timeScale = 0;
    }

    public override void Hide()
    {
        base.Hide();
        Time.timeScale = 1;
    }
}
