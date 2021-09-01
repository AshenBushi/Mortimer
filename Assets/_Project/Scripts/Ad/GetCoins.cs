using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class GetCoins : AdButton
{
    [SerializeField] private Player _player;
    
    protected override void HandleUserEarnReward(object sender, Reward e)
    {
        base.HandleUserEarnReward(sender, e);
        _player.AddMoney(500);
    }
}
