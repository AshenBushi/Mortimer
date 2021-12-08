using System;
using GoogleMobileAds.Api;

public class AdManager : Singleton<AdManager>
{
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

        MobileAds.Initialize(initStatus => { });
        
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

    private void InitializeRewarded()
    {
#if UNITY_ANDROID
        const string rewardId = "ca-app-pub-2719145281621012/2185608856"; 
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
        const string interstitialId = "ca-app-pub-2719145281621012/2652512718";
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

    public void ShowInterstitial()
    {
        if (!Interstitial.IsLoaded()) return;
        Interstitial.Show();
    }

    public void ShowRewardVideo()
    {
        if (!RewardedAd.IsLoaded()) return;
        RewardedAd.Show();
    }
}
