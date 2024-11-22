using NeuroSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ListItemClickedEvent : UnityEvent<int, SensorInfo> { }

public class ListView : MonoBehaviour
{
    [SerializeField] private GameObject _scrollView;
    [SerializeField] private GameObject _scrollViewContent;
    [SerializeField] private GameObject _scrollbar;
    [SerializeField] private GameObject _itemPrefab;

    public ListItemClickedEvent onClick;

    public void AddAll(IReadOnlyList<SensorInfo> infos) {

        foreach (Transform child in _scrollViewContent.transform)
            Destroy(child.gameObject);

        var index = 0;
        foreach (var device in infos)
        {
            var instance = Instantiate(_itemPrefab.gameObject, _scrollViewContent.transform);

            DeviceTab deviceTab = instance.GetComponent<DeviceTab>();

            if (deviceTab != null)
                deviceTab.Initialize(device, index, onClick);
            index++;
        }

        ShowList();
    }

    public void ShowList()
    {
        _scrollView.SetActive(true);
    }

    public void HideList()
    {
        _scrollView.SetActive(false);

        foreach (Transform child in _scrollViewContent.transform)
            Destroy(child.gameObject);
    }
}
