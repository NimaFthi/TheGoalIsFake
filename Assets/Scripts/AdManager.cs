using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;

    [Header("Banner Ads")]
    [SerializeField] private AdPosition bannerAdPosition;
    private string bannerAdUnitId;

    [Header("Interstitial Ads")]
    private string interstitialAdUnitId;
    public int deathNumberToShowAd = 5;
    public int numberOfLevelPassedToShowAd = 3;

    [Header("Settings")] 
    [SerializeField] private EnmDeviceType deviceType;
    [SerializeField] private List<AdDetail> adDetails;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var adDetail = adDetails.Find(x => x.deviceType == deviceType);
        if (adDetail!= null)
        {
            bannerAdUnitId = adDetail.bannerAdId;
            interstitialAdUnitId = adDetail.interstitialAdId;
        }
        else
        {
            Debug.Log("Ad Details Are Empty");
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
    }

    private AdRequest CreateAdRequest()
    {
        var request = new AdRequest.Builder().Build();
        return request;
    }

    public void RequestAndShowBannerAd()
    {
        bannerAd?.Destroy();
        
        bannerAd = new BannerView(bannerAdUnitId, AdSize.Leaderboard, bannerAdPosition);

        bannerAd.LoadAd(CreateAdRequest());
    }

    public void RequestInterstitial()
    {
        interstitialAd?.Destroy();
        
        interstitialAd = new InterstitialAd(interstitialAdUnitId);
        
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void DestroyAllAds()
    {
        bannerAd?.Destroy();
        interstitialAd?.Destroy();
    }
}

[Serializable]
public class AdDetail
{
    public EnmDeviceType deviceType;
    public string bannerAdId;
    public string interstitialAdId;
}

public enum EnmDeviceType
{
    Android,
    IOS
}