using UnityEngine;

public class PerkPanel : UIPanel
{
    public void Show()
    {
        base.Show(AnimationName.Slowly);
    }

    public void Hide()
    {
        base.Hide(AnimationName.Slowly);
    }
}
