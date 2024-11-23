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
    private int sumRelAtt = 0;
    private int count = 0;

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
                sumRelAtt += (int)_data.InstAttention - (int)_data.InstRelaxation;
                count++;
                //Debug.Log($"Attention percentage: {MathF.Round((float)_data.RelAttention)}%");
                //Debug.Log($"Relax percentage: {MathF.Round((float)_data.RelRelaxation, 2)}%)");
                
                
                /*_attentionPercentText.text = $"{MathF.Round((float)_data.RelAttention, 2)}%";
                _relaxPercentText.text = $"{MathF.Round((float)_data.RelRelaxation, 2)}%";
                _attentionRawText.text = $"{MathF.Round((float)_data.InstAttention, 2)}";
                _relaxRawText.text = $"{MathF.Round((float)_data.InstRelaxation, 2)}";
                _artSequenceText.text = $"{_artSequence}";
                _artBothSidesText.text = $"{_artBothSides}";
                _deltaPercentText.text = $"{MathF.Round((float)(_spectralData.Delta * 100), 2)}%";
                _thetaPercentText.text = $"{MathF.Round((float)(_spectralData.Theta * 100), 2)}%";
                _alphaPercentText.text = $"{MathF.Round((float)(_spectralData.Alpha * 100), 2)}%";
                _betaPercentText.text = $"{MathF.Round((float)(_spectralData.Beta * 100), 2)}%";
                _gammaPercentText.text = $"{MathF.Round((float)(_spectralData.Gamma * 100), 2)}%";
                _alphaRawText.text = $"{(int)_spectVals.Alpha}";
                _betaRawText.text = $"{(int)_spectVals.Beta}";
                _progressBar.fillAmount = _percentPB / 100;*/
            }
            yield return new WaitForSeconds(0.06f);
        }
    }

    private IEnumerator RangeUpdate()
    {
        while (started)
        {
            yield return new WaitForSeconds(5);
            sumRelAtt /= count;
            //Debug.Log($"{sumRelAtt}");
            
            Network.Instance.RequestAction(sumRelAtt);
            sumRelAtt = 0;
            count = 0;
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
            Debug.Log("Calibration finished");
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
