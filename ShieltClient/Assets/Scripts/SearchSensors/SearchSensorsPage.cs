using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NeuroSDK;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Android;
using System.Linq;

public class SearchSensorsPage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _startSearchButtonText;

    [SerializeField] private ListView _devicesList;

    [SerializeField] private TMP_Text _isConnectedText;
    
    private const string _startSearchText = "Start Search";
    private const string _stopSearchText = "Stop Search";

    private bool _isSearching = false;
    private bool isSearching
    {
        get {
            return _isSearching;
        }
        set {
            if (_isSearching != value) 
            {
                _isSearching = value;
                _startSearchButtonText.text = _isSearching ? _stopSearchText : _startSearchText;
            }
        }
    }

    private void SensorsFounded(IReadOnlyList<SensorInfo> sensors)
    {
        MainThreadDispatcher.RunOnMainThread(() => {
            _devicesList.AddAll(sensors);
        });
    }

    public void OnStartSearchButtonClicked()
    {
        if (!isSearching)
        {
#if UNITY_ANDROID
            Permission.RequestUserPermission("android.permission.BLUETOOTH");
            Permission.RequestUserPermission("android.permission.BLUETOOTH_ADMIN");
            Permission.RequestUserPermission("android.permission.BLUETOOTH_SCAN");
            Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
            Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
            Permission.RequestUserPermission("android.permission.ACCESS_COARSE_LOCATION");
#endif

            isSearching = true;
            BrainBitController.Instance.StartSearch(SensorsFounded);

        }
        else
        {
            isSearching = false;
            _devicesList.HideList();
            BrainBitController.Instance.StopSearch();
        }
    }

    public void deviceItemClicked(int index, SensorInfo info)
    {
        BrainBitController.Instance.CreateAndConnect(info, (state) =>
        {
            if (state == SensorState.StateInRange) {
                /*BackendManager.Instance.ToMenuPage();*/
                Debug.Log("Device connected");
                _isConnectedText.gameObject.SetActive(true);
            }
            else {
                Debug.Log("Device not connected!");
            
            }
        });
    }

    private void OnEnable()
    {
        Enter();
    }

    public void OnDisable()
    {
        Exit();
    }

    public void Enter()
    {
        isSearching = false;
    }

    public void Exit()
    {
        BrainBitController.Instance.StopSearch();

        _devicesList.HideList();
    }
}
