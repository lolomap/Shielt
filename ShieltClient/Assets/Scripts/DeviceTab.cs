using UnityEngine;
using TMPro;
using NeuroSDK;

public class DeviceTab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _addressText;

    private SensorInfo _info;
    private int _index;

    private ListItemClickedEvent onTabClicked = null;

    public void Initialize(SensorInfo info, int index, ListItemClickedEvent clicked)
    {
        _info = info;

        _nameText.text = info.Name;
        _addressText.text = info.Address;
        _index = index;

        onTabClicked = clicked;
    }

    public void OnClicked()
    {
        onTabClicked?.Invoke(_index, _info);
    }
}
