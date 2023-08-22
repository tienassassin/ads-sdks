using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    [SerializeField] private IronSourceManager iSManager;
    [SerializeField] private AdmobManager admobManager;
    
    [SerializeField] private Button btnShowInter;
    [SerializeField] private Button btnShowRewarded;
    [SerializeField] private Button btnShowRewardedInter;
    [SerializeField] private Button btnShowBanner;
    [SerializeField] private Button btnShowAOA;
    
    [SerializeField] private Text txtLog;
    [SerializeField] private ScrollRect scrLog;

    private void OnEnable()
    {
        Application.logMessageReceivedThreaded += RenderLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceivedThreaded -= RenderLog;
    }

    private void Start()
    {
        btnShowInter.onClick.AddListener(ShowInterstitial);
        btnShowRewarded.onClick.AddListener(ShowRewarded);
        btnShowRewardedInter.onClick.AddListener(ShowRewardedInterstitial);
        btnShowBanner.onClick.AddListener(ShowBanner);
        btnShowAOA.onClick.AddListener(ShowAppOpenAd);
    }
    
    private void RenderLog(string msg, string stackTrace, LogType type)
    {
        if (type != LogType.Error && type != LogType.Exception && !msg.Contains("iS >") && !msg.Contains("Admob >")) return;
        msg = Regex.Replace(msg, @"(AdInfo|And AdInfo).*", "");
        txtLog.text += $"\n+ {msg}";
        ScrollToBot();
    }

    private void ShowInterstitial()
    {
        iSManager.ShowInterstitial();
    }

    private void ShowRewarded()
    {
        iSManager.ShowRewardedAd();
    }

    private void ShowRewardedInterstitial()
    {
        
    }

    private void ShowBanner()
    {
        iSManager.ToggleBannerVisibility();
    }

    private void ShowAppOpenAd()
    {
        admobManager.ShowAOA();
    }
    
    public void ScrollToBot()
    {
        scrLog.normalizedPosition = Vector2.zero;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            admobManager.ShowAOA();
        }
    }
}
