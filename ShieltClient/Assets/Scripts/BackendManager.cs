using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance;

    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _deviceSearchScreen;
    [SerializeField] private GameObject _deviceInfoScreen;
    [SerializeField] private GameObject _signalScreen;
    [SerializeField] private GameObject _resistScreen;
    [SerializeField] private GameObject _emotionalScreen;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    public void ToSearchPage()
    {
        _deviceSearchScreen.gameObject.SetActive(true);
    }

    public void ToMenuPage()
    {
        _mainMenuScreen?.SetActive(true);
        _deviceSearchScreen?.SetActive(false);
    }

    public void ToDeviceInfoPage() 
    {
        _deviceInfoScreen.SetActive(true);
    }

    public void ToSignalPage() 
    {
        _signalScreen.SetActive(true);
    }

    public void ToResistPage() 
    {
        _resistScreen.SetActive(true);
    }
    public void ToEmotionalPage()
    {
        _emotionalScreen.SetActive(true);
    }


}
