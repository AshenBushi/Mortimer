using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class GetCoins : AdButton
{
    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        base.HandleUserEarnReward(sender, e);
        User.Instance.AddMoney(500);
    }
}
