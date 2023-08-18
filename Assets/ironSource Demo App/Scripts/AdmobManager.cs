using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
#if UNITY_ANDROID
    private string[] appOpenAdUnitIds = {
        "ca-app-pub-3940256099942544/3419835294",
        "ca-app-pub-3940256099942544/3419835294",
        "ca-app-pub-3940256099942544/3419835294",
        };
#elif UNITY_IOS
    private string[] appOpenAdUnitIds = {
        "",
        "",
        "",
        };
#else
    private string[] appOpenAdUnitIds = {
        "",
        "",
        "",
        };
#endif

    public bool IsAOAAvailable
    {
        get
        {
            return appOpenAd != null
                && appOpenAd.CanShowAd();
        }
    }

    private AppOpenAd appOpenAd;
    private int appOpenAdIndex = 0;
    private bool isShowingAOA = false;
    private bool isFirstShowAOA = false;

    private void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadAOA();
        });
    }

    private void LoadAOA()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }
        
        string id = appOpenAdUnitIds[appOpenAdIndex];
        Debug.Log($"Admob > AppOpenAd > Loading tier {appOpenAdIndex} - ID: {id}");

        var adRequest = new AdRequest();
        
        AppOpenAd.Load(id, adRequest, (ad, error) =>
        {
            if (error != null)
            {
                Debug.Log($"Admob > AppOpenAd > Load failed tier {appOpenAdIndex} - ID: {id}. Error: {error.GetMessage()}");
                appOpenAdIndex++;
                if (appOpenAdIndex >= appOpenAdUnitIds.Length) appOpenAdIndex = 0;
                LoadAOA();
                return;
            }
            
            Debug.Log($"Admob > AppOpenAd > Loaded tier {appOpenAdIndex} - ID: {id}");
            appOpenAd = ad;
            RegisterEventHandlers(ad);

            if (!isFirstShowAOA)
            {
                ShowAOA();
                isFirstShowAOA = true;
            }
        });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        ad.OnAdPaid += (adValue) =>
        {
            Debug.Log(String.Format("Admob > AppOpenAd > Paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Admob > AppOpenAd > Recorded an impression.");
        };
        
        ad.OnAdClicked += () =>
        {
            Debug.Log("Admob > AppOpenAd > Clicked.");
        };
        
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Admob > AppOpenAd > Opened.");
        };
        
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Admob > AppOpenAd > Closed.");
            LoadAOA();
        };
        
        ad.OnAdFullScreenContentFailed += (error) =>
        {
            Debug.LogError("Admob > AppOpenAd > Open failed. Error : " + error);
            LoadAOA();
        };
    }

    public void ShowAOA()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            appOpenAd.Show();
        }
        else
        {
            Debug.Log("Admob > AppOpenAd > Not ready");
        }
    }
}
