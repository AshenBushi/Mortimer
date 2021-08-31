using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    private float _timeSpendFromLastInterstitial = 30f;

    public bool IsInterstitialShowed { get; private set; }
    public InterstitialAd Interstitial { get; private set; }
    public RewardedAd RewardedAd{ get; private set; }

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });
        
        InitializeRewarded();
        InitializeInterstitial();
    }

    private void OnDisable()
    {
        Interstitial.OnAdClosed -= HandleOnAdClosed;
        RewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        RewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        RewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }

    private void Update()
    {
        if (_timeSpendFromLastInterstitial < 30)
            _timeSpendFromLastInterstitial += Time.deltaTime;
    }

    private void InitializeRewarded()
    {
#if UNITY_ANDROID
        const string rewardId = "ca-app-pub-8536869637526391/5119424843"; 
#elif UNITY_IPHONE
        const string rewardId = "";
#else
        const string rewardId = "unexpected_platform";
#endif
        
        var request = new AdRequest.Builder().Build();
        
        RewardedAd = new RewardedAd(rewardId);
        RewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        RewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        RewardedAd.OnAdClosed += HandleRewardedAdClosed;
        RewardedAd.LoadAd(request);
    }

    private void InitializeInterstitial()
    {
#if UNITY_ANDROID
        const string interstitialId = "ca-app-pub-8536869637526391/1180179839";
#elif UNITY_IPHONE
        const string interstitialId = "";
#else
        const string interstitialId = "unexpected_platform";
#endif
        
        var request = new AdRequest.Builder().Build();
        
        Interstitial = new InterstitialAd(interstitialId);
        Interstitial.OnAdClosed += HandleOnAdClosed;
        Interstitial.LoadAd(request);
    }
    
    private void HandleOnAdClosed(object sender, EventArgs e)
    {
        InitializeInterstitial();
        IsInterstitialShowed = true;
    }
    
    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        InitializeRewarded();
    }
    
    private void HandleUserEarnedReward(object sender, Reward e)
    {
        InitializeRewarded();
    }
    
    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        InitializeRewarded();
    }

    public bool ShowInterstitial()
    {
        if (!Interstitial.IsLoaded() || _timeSpendFromLastInterstitial < 30f) return false;
        Interstitial.Show();
        _timeSpendFromLastInterstitial = 0f;
        IsInterstitialShowed = false;
        return true;
    }

    public void ShowRewardVideo()
    {
        if (!RewardedAd.IsLoaded()) return;
        RewardedAd.Show();
    }
}
