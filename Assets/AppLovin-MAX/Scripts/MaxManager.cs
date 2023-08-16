using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxManager : MonoBehaviour
{
    [SerializeField] private string sdkKey = "MAX_SDK_KEY";
    [SerializeField] private AdUnitData androidAdUnitData;
    [SerializeField] private AdUnitData iosAdUnitData;
    
    private string appOpenAdUnitId;
    private string interstitialAdUnitId;
    private string rewardedAdUnitId;
    private string rewardedInterstitialAdUnitId;
    private string bannerAdUnitId;

    private bool isBannerShowing;

    private int aoaRetryAttempt;
    private int interstitialRetryAttempt;
    private int rewardedRetryAttempt;
    private int rewardedInterstitialRetryAttempt;

    private void Start()
    {
#if UNITY_EDITOR
        appOpenAdUnitId = "DUMMY_AD_UNIT_ID";
        interstitialAdUnitId = "DUMMY_AD_UNIT_ID";
        rewardedAdUnitId = "DUMMY_AD_UNIT_ID";
        rewardedInterstitialAdUnitId = "DUMMY_AD_UNIT_ID";
        bannerAdUnitId = "DUMMY_AD_UNIT_ID";
#elif UNITY_ANDROID
        appOpenAdUnitId = androidAdUnitData.appOpenAdUnitId;
        interstitialAdUnitId = androidAdUnitData.interstitialAdUnitId;
        rewardedAdUnitId = androidAdUnitData.rewardedAdUnitId;
        rewardedInterstitialAdUnitId = androidAdUnitData.rewardedInterstitialAdUnitId;
        bannerAdUnitId = androidAdUnitData.bannerAdUnitId;
#elif UNITY_IOS
        appOpenAdUnitId = iosAdUnitData.appOpenAdUnitId;
        interstitialAdUnitId = iosAdUnitData.interstitialAdUnitId;
        rewardedAdUnitId = iosAdUnitData.rewardedAdUnitId;
        rewardedInterstitialAdUnitId = iosAdUnitData.rewardedInterstitialAdUnitId;
        bannerAdUnitId = iosAdUnitData.bannerAdUnitId;
#endif

        MaxSdkCallbacks.OnSdkInitializedEvent += configuration =>
        {
            Debug.Log("MAX SDK Initialized");
            
            InitializeAppOpenAds();
            InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeRewardedInterstitialAds();
            InitializeBannerAds();
        };
        
        MaxSdk.SetSdkKey(sdkKey);
        MaxSdk.InitializeSdk();
    }


    //====================================================================================================== App Open Ad
    #region AOA Methods

    private void InitializeAppOpenAds()
    {
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenAdLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenAdFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += AppOpenAdFailedToDisplayEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenAdDismissedEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAppOpenAdRevenuePaidEvent;

        LoadAppOpenAd();
    }

    private void LoadAppOpenAd()
    {
        MaxSdk.LoadAppOpenAd(appOpenAdUnitId);
    }

    public void ShowAppOpenAd()
    {
        if (MaxSdk.IsAppOpenAdReady(appOpenAdUnitId))
        {
            MaxSdk.ShowAppOpenAd(appOpenAdUnitId);
        }
        else
        {
            Debug.Log("MAX > AppOpenAd > Not Ready");
        }
    }
    
    private void OnAppOpenAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > AppOpenAd > Loaded");
        aoaRetryAttempt = 0;
    }
    
    private void OnAppOpenAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        aoaRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, aoaRetryAttempt));
        Debug.Log("MAX > AppOpenAd > Load failed: " + errorInfo.Code + 
                  "\nRetrying in " + retryDelay + "s");
        
        Invoke(nameof(LoadAppOpenAd), (float) retryDelay);
    }
    
    private void AppOpenAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > AppOpenAd > Display failed: " + errorInfo.Code);
        LoadAppOpenAd();
    }
    
    private void OnAppOpenAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > AppOpenAd > Dismissed");
        LoadAppOpenAd();
    }
    
    private void OnAppOpenAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > AppOpenAd > Revenue paid");
    }

    #endregion
    
    
    //===================================================================================================== Interstitial
    #region Interstitial Methods

    private void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(interstitialAdUnitId);
    }
    
    public void ShowInterstitial()
    {
        if (MaxSdk.IsInterstitialReady(interstitialAdUnitId))
        {
            MaxSdk.ShowInterstitial(interstitialAdUnitId);
        }
        else
        {
            Debug.Log("MAX > Interstitial > Not ready");
        }
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Interstitial > Loaded");
        interstitialRetryAttempt = 0;
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));
        Debug.Log("MAX > Interstitial > Load failed: " + errorInfo.Code +
                  "\nRetrying in " + retryDelay + "s");

        Invoke(nameof(LoadInterstitial), (float)retryDelay);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Interstitial > Display failed: " + errorInfo.Code);
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Interstitial > Dismissed");
        LoadInterstitial();
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Interstitial > Revenue paid");
    }

    #endregion
    
    
    //========================================================================================================= Rewarded
    #region Rewarded Methods

    private void InitializeRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(rewardedAdUnitId);
    }

    public void ShowRewardedAd()
    {
        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {
            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }
        else
        {
            Debug.Log("MAX > Rewarded > Not ready");
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Loaded");
        rewardedRetryAttempt = 0;
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));
        Debug.Log("MAX > Rewarded > Load failed: " + errorInfo.Code + 
                  "\nRetrying in " + retryDelay + "s");

        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Display failed: " + errorInfo.Code);
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Displayed");
    }
    
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Dismissed");
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Received reward");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded > Revenue paid");
    }

    #endregion
    
    
    //============================================================================================ Rewarded Interstitial
    #region Rewarded Interstitial Methods

    private void InitializeRewardedInterstitialAds()
    {
        MaxSdkCallbacks.RewardedInterstitial.OnAdLoadedEvent += OnRewardedInterstitialAdLoadedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdLoadFailedEvent += OnRewardedInterstitialAdFailedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayFailedEvent += OnRewardedInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdDisplayedEvent += OnRewardedInterstitialAdDisplayedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdClickedEvent += OnRewardedInterstitialAdClickedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdHiddenEvent += OnRewardedInterstitialAdDismissedEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdReceivedRewardEvent += OnRewardedInterstitialAdReceivedRewardEvent;
        MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += OnRewardedInterstitialAdRevenuePaidEvent;

        LoadRewardedInterstitialAd();
    }

    private void LoadRewardedInterstitialAd()
    {
        MaxSdk.LoadRewardedInterstitialAd(rewardedInterstitialAdUnitId);
    }

    public void ShowRewardedInterstitialAd()
    {
        if (MaxSdk.IsRewardedInterstitialAdReady(rewardedInterstitialAdUnitId))
        {
            MaxSdk.ShowRewardedInterstitialAd(rewardedInterstitialAdUnitId);
        }
        else
        {
            Debug.Log("MAX > Rewarded Inter > Not ready");
        }
    }

    private void OnRewardedInterstitialAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Loaded");
        rewardedInterstitialRetryAttempt = 0;
    }

    private void OnRewardedInterstitialAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedInterstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedInterstitialRetryAttempt));
        Debug.Log("MAX > Rewarded Inter > Load failed: " + errorInfo.Code + 
                  "\nRetrying in " + retryDelay + "s");
        
        Invoke(nameof(LoadRewardedInterstitialAd), (float) retryDelay);
    }

    private void OnRewardedInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Display failed");
        LoadRewardedInterstitialAd();
    }
    
    private void OnRewardedInterstitialAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Displayed");
    }

    private void OnRewardedInterstitialAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Clicked");
    }
    
    private void OnRewardedInterstitialAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Dismissed");
        LoadRewardedInterstitialAd();
    }
    
    private void OnRewardedInterstitialAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Received reward");
    }

    private void OnRewardedInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Rewarded Inter > Revenue paid");
    }

    #endregion
    
    
    //=========================================================================================================== Banner
    #region Banner Methods

    private void InitializeBannerAds()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.TopCenter);
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);
    }
    
    public void ToggleBannerVisibility()
    {
        if (!isBannerShowing)
        {
            MaxSdk.ShowBanner(bannerAdUnitId);
        }
        else
        {
            MaxSdk.HideBanner(bannerAdUnitId);
        }

        isBannerShowing = !isBannerShowing;
    }
    
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Banner > Loaded");
    }
    
    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        Debug.Log("MAX > Banner > Load failed: " + errorInfo.Code);
    }
    
    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Banner > Clicked");
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("MAX > Banner > Revenue paid");
    }

    #endregion
}

[Serializable]
public struct AdUnitData
{
    public string appOpenAdUnitId;
    public string interstitialAdUnitId;
    public string rewardedAdUnitId;
    public string rewardedInterstitialAdUnitId;
    public string bannerAdUnitId;
} 
