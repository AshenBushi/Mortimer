using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class Reroll : AdButton
{
    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        base.HandleUserEarnReward(sender, e);
        SkillsHandler.Instance.GiveRandomSkills();
    }
}
