using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
public class AdsInitializer : MonoBehaviour,IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
    [SerializeField] Button AdForTicket;
    [SerializeField] Button AdForRevive;
    [SerializeField ]string androidGameId;
    [SerializeField] bool testMode;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _androidAdUnitId2 = "Rewarded_Android2";
    [SerializeField] string _androidAdUnitId3 = "Interstitial_Android";
    string _adUnitId = null; // This will remain null for unsupported platforms
    string _adUnitId2 = null;
    string forcedAdId = null;
    public GameManage manage;
    public PlayerMotor motor;
    [SerializeField] private int doNotCollideTime;

    public void Awake()
    {
        _adUnitId = _androidAdUnitId;
        _adUnitId2 = _androidAdUnitId2;
        forcedAdId = _androidAdUnitId3;
        //Disable the button until the ad is ready to show:
        AdForTicket.interactable = false;
        AdForRevive.interactable = false;
        InitializeAds();
        
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        Debug.Log("Loading Ad: " + _adUnitId2);
        Advertisement.Load(_adUnitId2, this);
    }
   public void InitializeAds()
    {
        Advertisement.Initialize(androidGameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Loading Ad:" + forcedAdId);
        LoadInerstitialAd();

         Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);

        Debug.Log("Loading Ad: " + _adUnitId2);
        Advertisement.Load(_adUnitId2, this);
    }

   

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Unity Ads initialization Failed.");
    }
    public void LoadInerstitialAd()
    {
        Advertisement.Load(forcedAdId,this);
    }
    
    public void OnUnityAdsAdLoaded(string adUnitId)
    {

        //// Button 1 ////
        if (adUnitId.Equals(_adUnitId))
        {

            // Configure the button to call the ShowAd() method when clicked:
            AdForTicket.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            AdForTicket.interactable = true;


        }


        /// ad 2 ////         
        if (adUnitId.Equals(_adUnitId2))
        {

            // Configure the button to call the ShowAd() method when clicked:
            AdForRevive.onClick.AddListener(ShowAd2);
            // Enable the button for users to click:
            AdForRevive.interactable = true;
        }
    }
    public void ShowForcedAd()
    {
        Advertisement.Show(forcedAdId,this);
        
        PlayerPrefs.SetInt("forcedAdCount",PlayerPrefs.GetInt("forcedAdCount")+1);
        // Load another ad
        LoadInerstitialAd();
    }
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        AdForTicket.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
        PlayerPrefs.SetInt("rewardedAd1Count",PlayerPrefs.GetInt("rewardedAd1Count")+1);
        Debug.Log(PlayerPrefs.GetInt("rewardedAd1Count"));
        // Load another ad:
        Advertisement.Load(_adUnitId, this);
    }
    public void ShowAd2()
    {
        // Disable the button:
        AdForRevive.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId2, this);
        // Load another ad:
        PlayerPrefs.SetInt("rewardedAd2Count",PlayerPrefs.GetInt("rewardedAd2Count")+1);
        Debug.Log(PlayerPrefs.GetInt("rewardedAd2Count"));
        Advertisement.Load(_adUnitId2, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Unity Ads Load Failed.");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
        Time.timeScale = 0;
        manage.isShowingAd = true;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }
 
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        manage.isShowingAd = false;
        if(adUnitId.Equals(forcedAdId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && !(PlayerPrefs.GetInt("forcedAdCount")%2==0))
        {
            Debug.Log("Unity Ads Forced Ad Completed");
            ShowForcedAd();
        }
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && !(PlayerPrefs.GetInt("rewardedAd1Count")%3==0))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            ShowAd(); 
        }
        if (adUnitId.Equals(_adUnitId2) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && !(PlayerPrefs.GetInt("rewardedAd2Count")%3==0))
        {
            Debug.Log("Unity Ads Rewarded2 Ad Completed");
            PlayerPrefs.SetInt("rewardedAd2Count",PlayerPrefs.GetInt("rewardedAd2Count")+1);
                    Debug.Log(PlayerPrefs.GetInt("rewardedAd2Count"));
            ShowAd2();  
        }
        if(adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && (PlayerPrefs.GetInt("rewardedAd1Count")%3==0))
        {
            PlayerPrefs.SetInt("ticket", PlayerPrefs.GetInt("ticket") + 1);
            manage.ticketText.text = PlayerPrefs.GetInt("ticket").ToString();
            Debug.Log("you get reward");
        }

        if(adUnitId.Equals(_adUnitId2) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && (PlayerPrefs.GetInt("rewardedAd2Count")%3==0))
        {
            motor.OnReviveWithAdd();
            Debug.Log("you get reward 2 ");
            StartCoroutine(DoNotCollideForSeconds(doNotCollideTime));
        }
    }
    void OnDestroy()
    {
        // Clean up the button listeners:
        AdForTicket.onClick.RemoveAllListeners();
        AdForRevive.onClick.RemoveAllListeners();
    }
    private IEnumerator DoNotCollideForSeconds(int seconds)
    {
            Debug.Log("Not Collidable");
            Physics.IgnoreLayerCollision(0,3);
            yield return new WaitForSeconds(3+seconds);
            Physics.IgnoreLayerCollision(0,3,false);
            Debug.Log("Collidable");
    } 
}
