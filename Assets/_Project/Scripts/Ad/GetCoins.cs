using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using GoogleMobileAds.Api;
using UnityEngine;

public class GetCoins : AdButton
{
    [SerializeField] private Player _player;
    
    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        base.HandleUserEarnReward(sender, e);
        Wallet.Instance.AddMoney(500);
        FirebaseAnalytics.LogEvent("ad_get_coins");
    }
}
