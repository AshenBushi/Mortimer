using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdButton : MonoBehaviour
{
    protected Button Button;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    public IEnumerator Init()
    {
        yield return new WaitForSeconds(0f);
        
        AdManager.Instance.RewardedAd.OnAdFailedToLoad += HandleFailedToLoad;
        AdManager.Instance.RewardedAd.OnAdLoaded += HandleAdLoaded;
    }
    
    private void OnEnable()
    {
        StartCoroutine(Init());
    }

    private void OnDisable()
    {
        AdManager.Instance.RewardedAd.OnAdFailedToLoad -= HandleFailedToLoad;
        AdManager.Instance.RewardedAd.OnAdLoaded -= HandleAdLoaded;
    }

    private void HandleAdLoaded(object sender, EventArgs e)
    {
        Button.interactable = true;
    }
    
    private void HandleFailedToLoad(object sender, AdFailedToLoadEventArgs adFailedToLoadEventArgs)
    {
        Button.interactable = false;
    }
    
    protected virtual void HandleFailedToShow(object sender, AdErrorEventArgs e)
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }

    protected virtual void HandleUserEarnReward(object sender, Reward e)
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward -= HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow -= HandleFailedToShow;
    }
    
    public virtual void WatchAd()
    {
        AdManager.Instance.RewardedAd.OnUserEarnedReward += HandleUserEarnReward;
        AdManager.Instance.RewardedAd.OnAdFailedToShow += HandleFailedToShow;
        AdManager.Instance.ShowRewardVideo();
        Button.interactable = false;
    }

    public void TryEnableButton()
    {
        if (AdManager.Instance.RewardedAd.IsLoaded())
        {
            Button.interactable = true;
        }
    }
}