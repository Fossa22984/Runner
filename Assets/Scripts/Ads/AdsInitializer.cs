using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private Rewarded res;

    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode = true;
    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ?
            _iOSGameId : _androidGameId;

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }

        //#if UNITY_IOS
        //            _gameId = _iOSGameId;
        //#elif UNITY_ANDROID
        //        _gameId = _androidGameId;
        //#elif UNITY_EDITOR
        //            _gameId = _androidGameId; //Only for testing the functionality in the Editor
        //#endif
        //        if (!Advertisement.isInitialized && Advertisement.isSupported)
        //        {
        //            Advertisement.Initialize(_gameId, _testMode, this);
        //        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        res.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
