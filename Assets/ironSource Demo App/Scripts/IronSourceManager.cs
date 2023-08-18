using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


// Example for IronSource Unity.
public class IronSourceManager : MonoBehaviour
{
    private bool isBannerShowing;

    private int[] interstitialRetryDelay = { 0, 5, 8, 16, 32, 64 };
    private int interstitialRetryAttempt = 0;

    public void Start()
    {

#if UNITY_ANDROID
        string appKey = "85460dcd";
#elif UNITY_IPHONE
        string appKey = "8545d445";
#else
        string appKey = "unexpected_platform";
#endif



        Debug.Log("iS > IronSource.Agent.validateIntegration");
        IronSource.Agent.validateIntegration();

        Debug.Log("iS > unity version" + IronSource.unityVersion());

        // SDK init
        Debug.Log("iS > IronSource.Agent.init");
        IronSource.Agent.init(appKey);

        LoadInterstitial();
        ToggleBannerVisibility();
    }

    void OnEnable()
    {

        //Add Init Event
        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        
        //Add Rewarded Video Events
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

        //Add Rewarded Video DemandOnly Events
        IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent += RewardedVideoAdLoadedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
        IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent += RewardedVideoAdLoadFailedDemandOnlyEvent;


        // Add Offerwall Events
        IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
        IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
        IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
        IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
        IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
        IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;


        // Add Interstitial Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        // Add Interstitial DemandOnly Events
        IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
        IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;


        // Add Banner Events
        IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

        //Add ImpressionSuccess Event
        IronSourceEvents.onImpressionSuccessEvent += ImpressionSuccessEvent;
        IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataReadyEvent;


        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;


        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;

    }

    void OnApplicationPause(bool isPaused)
    {
        Debug.Log("iS > OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void RetryToLoadInterstitial()
    {
        interstitialRetryAttempt++;
        float retryDelay = interstitialRetryDelay[Mathf.Min(interstitialRetryAttempt, interstitialRetryDelay.Length)];
        Invoke(nameof(LoadInterstitial), interstitialRetryDelay[interstitialRetryAttempt]);
    }

    private void LoadInterstitial()
    {
        Debug.Log("iS > Interstitial > Loading...");
        IronSource.Agent.loadInterstitial();
    }

    public void ShowInterstitial()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            Debug.Log("iS > Interstitial > Not ready");
        }
    }

    public void ShowRewardedAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            Debug.Log("iS > Rewarded > Not ready");
        }
    }

    public void ToggleBannerVisibility()
    {
        if (!isBannerShowing)
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }
        else
        {
            IronSource.Agent.destroyBanner();
        }

        isBannerShowing = !isBannerShowing;
    }

    public void OnGUI()
    {
        return;
        GUI.backgroundColor = Color.blue;
        GUI.skin.button.fontSize = (int)(0.035f * Screen.width);

        Rect showOfferwallButton = new Rect(0.10f * Screen.width, 0.25f * Screen.height, 0.80f * Screen.width, 0.08f * Screen.height);
        if (GUI.Button(showOfferwallButton, "Show Offerwall"))
        {
            if (IronSource.Agent.isOfferwallAvailable())
            {
                IronSource.Agent.showOfferwall();
            }
            else
            {
                Debug.Log("IronSource.Agent.isOfferwallAvailable - False");
            }
        }
        
    }
    
    #region Init callback handlers

    void SdkInitializationCompletedEvent()
    {
        Debug.Log("unity-script: I got SdkInitializationCompletedEvent");
    }

    #endregion

    #region AdInfo Rewarded Video
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo) 
    {
         Debug.Log("iS > Rewarded > Opened. AdInfo " + adInfo.ToString());
    }
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        Debug.Log("iS > Rewarded > Closed. AdInfo " + adInfo.ToString());
    }
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("iS > Rewarded > Available. AdInfo " + adInfo.ToString());
    }
    void RewardedVideoOnAdUnavailable()
    {
        Debug.Log("iS > Rewarded > Unavailable.");
    }
    void RewardedVideoOnAdShowFailedEvent(IronSourceError ironSourceError,IronSourceAdInfo adInfo)
    {
        Debug.Log("iS > Rewarded > Show failed. Error"+ironSourceError.ToString() + "And AdInfo " + adInfo.ToString());
    }
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement,IronSourceAdInfo adInfo)
    {
        Debug.Log("iS > Rewarded > Received. Placement" + ironSourcePlacement.ToString()+ "And AdInfo " + adInfo.ToString());
    }
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {
        Debug.Log("iS > Rewarded > Clicked. Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
    }

    #endregion



    #region RewardedAd callback handlers

    void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
    {
        Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
    }

    void RewardedVideoAdOpenedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
    }

    void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
        
    }

    void RewardedVideoAdClosedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
    }

    void RewardedVideoAdStartedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
    }

    void RewardedVideoAdEndedEvent()
    {
        Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
    }

    void RewardedVideoAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
    }

    /************* RewardedVideo DemandOnly Delegates *************/

    void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
    {
        
        Debug.Log("unity-script: I got RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        
        Debug.Log("unity-script: I got RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
    }

    void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
    }


    #endregion

    #region AdInfo Interstitial

    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Loaded. AdInfo " + adInfo.ToString());
        interstitialRetryAttempt = 0;
    }

    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) {
        Debug.Log("iS > Interstitial > Load failed. Error " + ironSourceError.ToString());
        RetryToLoadInterstitial();
    }

    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Opened. AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Clicked. AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Show succeeded. AdInfo " + adInfo.ToString());
    }

    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Show failed. Error " +ironSourceError.ToString()+ " And AdInfo " + adInfo.ToString());
        LoadInterstitial();
    }

    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Interstitial > Closed. AdInfo " + adInfo.ToString());
        LoadInterstitial();
    }


    #endregion

    #region Interstitial callback handlers

    void InterstitialAdReadyEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
    }

    void InterstitialAdShowFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void InterstitialAdClickedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
    }

    void InterstitialAdOpenedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
    }

    void InterstitialAdClosedEvent()
    {
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
    }

    /************* Interstitial DemandOnly Delegates *************/

    void InterstitialAdReadyDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
    {
        Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
    }

    void InterstitialAdClickedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
    }

    void InterstitialAdClosedDemandOnlyEvent(string instanceId)
    {
        Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
    }




    #endregion

    #region Banner AdInfo

    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Banner > Loaded. AdInfo " + adInfo.ToString());
    }

    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) {
        Debug.Log("iS > Banner > Load failed. Error " + ironSourceError.ToString());
    }

    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Banner > Clicked. AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Banner > Presented. AdInfo " + adInfo.ToString());
    }

    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Banner > Dismissed. AdInfo " + adInfo.ToString());
    }

    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) {
        Debug.Log("iS > Banner > Left application. AdInfo " + adInfo.ToString());
    }

    #endregion

    #region Banner callback handlers

    void BannerAdLoadedEvent()
    {
        Debug.Log("unity-script: I got BannerAdLoadedEvent");
    }

    void BannerAdLoadFailedEvent(IronSourceError error)
    {
        Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
    }

    void BannerAdClickedEvent()
    {
        Debug.Log("unity-script: I got BannerAdClickedEvent");
    }

    void BannerAdScreenPresentedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
    }

    void BannerAdScreenDismissedEvent()
    {
        Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
    }

    void BannerAdLeftApplicationEvent()
    {
        Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
    }

    #endregion


    #region Offerwall callback handlers

    void OfferwallOpenedEvent()
    {
        Debug.Log("I got OfferwallOpenedEvent");
    }

    void OfferwallClosedEvent()
    {
        Debug.Log("I got OfferwallClosedEvent");
    }

    void OfferwallShowFailedEvent(IronSourceError error)
    {
        Debug.Log("I got OfferwallShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
    {
        Debug.Log("I got OfferwallAdCreditedEvent, current credits = " + dict["credits"] + " totalCredits = " + dict["totalCredits"]);
        
    }

    void GetOfferwallCreditsFailedEvent(IronSourceError error)
    {
        Debug.Log("I got GetOfferwallCreditsFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
    }

    void OfferwallAvailableEvent(bool canShowOfferwal)
    {
        Debug.Log("I got OfferwallAvailableEvent, value = " + canShowOfferwal);
        
    }

    #endregion

    #region ImpressionSuccess callback handler

    void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);
    }

    void ImpressionDataReadyEvent(IronSourceImpressionData impressionData)
    {
        Debug.Log("unity - script: I got ImpressionDataReadyEvent ToString(): " + impressionData.ToString());
        Debug.Log("unity - script: I got ImpressionDataReadyEvent allData: " + impressionData.allData);
    }

    #endregion

}
