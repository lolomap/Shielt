using SignalMath;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotionalPage : MonoBehaviour
{
    public static EmotionalPage Instance;

    private MindData _data;
    private bool _artSequence;
    private bool _artBothSides;
    private SpectralDataPercents _spectralData;
    private RawSpectVals _spectVals;
    private float _percentPB =0;
    private readonly object locker = new object();
    private EmotionsController _emotionController;
    private IEnumerator _updateEmotionalCoroutine;
    private int maxRelax = 0;
    private int maxAttention = 0;
    private bool _scanning = false;
    [SerializeField] private TMP_Text messageToPlayers;

    private bool _started = false;
    private bool started
    {
        get { return _started; }
        set
        {
            if (value != _started)
            {
                _started = value;
                //_buttonText.text = _started ? "Stop" : "Start";
            }

        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator UpdateValues()
    {
        
        while (true)
        {
            lock (locker)
            {
                if (_scanning)
                {
                    //sumRelAtt += (int) _data.InstAttention - (int) _data.InstRelaxation;
                    //count++;

                    if (_data.RelRelaxation > maxRelax) { maxRelax = (int)_data.RelRelaxation; }
                    if (_data.RelAttention > maxAttention) {  maxAttention = (int)_data.RelAttention; }

                }
            }
            yield return new WaitForSeconds(0.06f);
        }
    }


    private IEnumerator RangeUpdate()
    {
        while (started)
        {
            _scanning = false;
            messageToPlayers.text = "Let's get started";
            yield return new WaitForSeconds(3);
            messageToPlayers.text = "Relax or attention";
            _scanning = true;
            
            yield return new WaitForSeconds(5);

            int value = Math.Max(maxRelax, maxAttention);
            int res = value / 2;
            bool isDef = value == maxRelax;
            Debug.Log((isDef ? "DEF " : "ATK ") + $"{res}");
            
            Network.Instance.RequestAction(res, isDef);
            maxRelax = 0;
            maxAttention = 0;
        }
    }

    public void StartScanning()
    {
        if (started)
        {
            BrainBitController.Instance.StopSignal();
            started = false;
        }
        else
        {
            _emotionController.StartCalibration();
            BrainBitController.Instance.StartSignal((samples) => {
                _emotionController.ProcessData(samples);
            });
            started = true;
            StartCoroutine(RangeUpdate());
        }
    }

    private void calibrationCallback(int progress)
    {
        _percentPB = progress;
    }

    private void mindDataCallback(MindData data)
    {
        _data=data;
    }

    private void isArtifactedSequenceCallback(bool artifacted)
    {
        _artSequence = artifacted;
    }

    private void isBothSidesArtifactedCallback(bool artifacted)
    {
        _artBothSides = artifacted;
    }

    private void lastSpectralDataCallback(SpectralDataPercents spectralData)
    {
        _spectralData = spectralData;
    }

    private void rawSpectralDataCallback(RawSpectVals spectVals)
    {
        _spectVals = spectVals;
    }

    private void OnEnable()
    {
        Enter();
    }

    private void OnDisable()
    {
        Exit();
    }

    public void Enter()
    {
        _emotionController = new EmotionsController();
        _emotionController.progressCalibrationCallback = calibrationCallback;
        _emotionController.isArtefactedSequenceCallback = isArtifactedSequenceCallback;
        _emotionController.isBothSidesArtifactedCallback = isBothSidesArtifactedCallback;
        _emotionController.lastMindDataCallback = mindDataCallback;
        _emotionController.lastSpectralDataCallback = lastSpectralDataCallback;
        _emotionController.rawSpectralDataCallback = rawSpectralDataCallback;

        _updateEmotionalCoroutine = UpdateValues();
        StartCoroutine(_updateEmotionalCoroutine);
    }

    public void Exit()
    {
        if (started)
        {
            started = !started;
        }
        StopCoroutine(_updateEmotionalCoroutine);
        BrainBitController.Instance.StopSignal();
        
        _data.RelAttention = 0;
        _data.RelRelaxation = 0;
        _data.InstAttention = 0;
        _data.InstRelaxation = 0;
        _artSequence = false;
        _artBothSides = false;
        _spectralData.Delta =0;
        _spectralData.Theta = 0;
        _spectralData.Alpha = 0;
        _spectralData.Beta = 0;
        _spectralData.Gamma = 0;
        _spectVals.Alpha = 0;
        _spectVals.Beta = 0;
    }
}
