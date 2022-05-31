using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using TMPro.EditorUtilities;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    
    //ad options
    [SerializeField] private AdPosition bannerAdPosition;
    public int deathNumberToShowAd = 5;
    public int numberOfLevelPassedToShowAd = 3;

    private void Start()
    {
        // RequestBanner();
    }

    private AdRequest CreateAdRequest()
    {
        var request = new AdRequest.Builder().Build();
        return request;
    }

    private void RequestBanner()
    {
        bannerAd?.Destroy();

        var addUnitId = "ca-app-pub-6453468160873909/3287143702";
        bannerAd = new BannerView(addUnitId, AdSize.Leaderboard, bannerAdPosition);
        
        bannerAd.LoadAd(CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        var adUnitId = "ca-app-pub-6453468160873909/1630694522";

        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(adUnitId);
        
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }
    
    
}