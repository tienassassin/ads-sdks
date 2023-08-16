using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    [SerializeField] private Button btnShowInter;
    [SerializeField] private Button btnShowRewarded;
    [SerializeField] private Button btnShowRewardedInter;
    [SerializeField] private Button btnShowBanner;
    [SerializeField] private Button btnShowAOA;

    private void Start()
    {
        btnShowInter.onClick.AddListener(ShowInterstitial);
        btnShowRewarded.onClick.AddListener(ShowRewarded);
        btnShowRewardedInter.onClick.AddListener(ShowRewardedInterstitial);
        btnShowBanner.onClick.AddListener(ShowBanner);
        btnShowAOA.onClick.AddListener(ShowAppOpenAd);
    }

    private void ShowInterstitial()
    {
        
    }

    private void ShowRewarded()
    {
        
    }

    private void ShowRewardedInterstitial()
    {
        
    }

    private void ShowBanner()
    {
        
    }

    private void ShowAppOpenAd()
    {
        
    }
}
