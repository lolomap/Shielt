using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class BrainBitController
{
    #region Singleton
    private static readonly Lazy<BrainBitController> lazy =
            new Lazy<BrainBitController>(() => new BrainBitController());

    public static BrainBitController Instance { get { return lazy.Value; } }

    private BrainBitController() { }
    #endregion

    #region Scanner
    private Scanner scanner = null;

    private Action<IReadOnlyList<SensorInfo>> _sensorsChanged = null;

    public void StartSearch(Action<IReadOnlyList<SensorInfo>> sensorsChanged)
    {
        if (scanner == null)
        {
            scanner = new Scanner(SensorFamily.SensorLEBrainBit, SensorFamily.SensorLEBrainBitBlack);
        }
        _sensorsChanged = sensorsChanged;
        scanner.EventSensorsChanged += Scanner_EventSensorsChanged;
        scanner?.Start();
    }

    public void Scanner_EventSensorsChanged(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
    {
        _sensorsChanged?.Invoke(sensors);
    }

    public void StopSearch()
    {
        scanner?.Stop();
    }

    public IReadOnlyList<SensorInfo> AvailableSensors => scanner?.Sensors;
    #endregion

    #region Sensor state
    private BrainBitSensor sensor = null;

    public Action<SensorState> connectionStateChanged = null;
    public Action<int> batteryChanged = null;

    public void CreateAndConnect(SensorInfo sensorInfo, Action<SensorState> onConnectionResult)
    {
        sensor = scanner.CreateSensor(sensorInfo) as BrainBitSensor;

        if (sensor != null)
        {
            sensor.EventSensorStateChanged += Sensor_EventSensorStateChanged;
            sensor.EventBatteryChanged += Sensor_EventBatteryChanged;

            connectionStateChanged?.Invoke(SensorState.StateInRange);
            onConnectionResult?.Invoke(SensorState.StateInRange);
        }
        else
        {
            onConnectionResult?.Invoke(SensorState.StateOutOfRange);
        }
    }

    private void Sensor_EventBatteryChanged(ISensor sensor, int battPower)
    {
        batteryChanged?.Invoke(battPower);
    }

    private void Sensor_EventSensorStateChanged(ISensor sensor, SensorState sensorState)
    {
        connectionStateChanged?.Invoke(sensorState);
    }

    public void ConnectCurrent(Action<SensorState> onConnectionResult)
    {
        if (sensor?.State == SensorState.StateOutOfRange)
        {
            Task.Factory.StartNew(() => {
                if (sensor != null) 
                {
                    sensor?.Connect();
                    onConnectionResult?.Invoke(sensor.State);
                }
            });
        }
    }

    public void DisconnectCurrent()
    {
        if (sensor?.State == SensorState.StateInRange)
            sensor?.Disconnect();
    }

    public void closeSensor()
    {
        sensor?.Dispose();
    }

    public SensorState ConnectionState
    { 
        get 
        {
            if (sensor != null) 
                return sensor.State;
            else
                return SensorState.StateOutOfRange;
        } 
    }
    #endregion

    #region Parameters
    public string FullInfo() {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Features\n");
        var featuresLines = SensorInfoProvider.GetSensorFeatures(sensor).Select(x => x.ToString());
        stringBuilder.Append(string.Join(Environment.NewLine, featuresLines));
        stringBuilder.Append("\nCommands\n");
        var commandLines = SensorInfoProvider.GetSensorCommands(sensor).Select(x => x.ToString());
        stringBuilder.Append(string.Join(Environment.NewLine, commandLines));
        stringBuilder.Append("\nParameters\n");
        var parameterLines = SensorInfoProvider.GetBrainBitSensorParameters(sensor).Select
            (kvp => kvp.Key.ToString() + " - " + kvp.Value);
        stringBuilder.Append(string.Join(Environment.NewLine, parameterLines));
        return stringBuilder.ToString();
    }

    #endregion

    #region Signal
    private Action<BrainBitSignalData[]> _signalReceived = null;
    public void StartSignal(Action<BrainBitSignalData[]>  signalReceived)
    {
        _signalReceived = signalReceived;
        sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;
        executeCommand(SensorCommand.CommandStartSignal);
    }

    private void Sensor_EventBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
    {
        _signalReceived?.Invoke(data);
    }

    public void StopSignal()
    {
        sensor.EventBrainBitSignalDataRecived -= Sensor_EventBrainBitSignalDataRecived;
        _signalReceived = null;
        executeCommand(SensorCommand.CommandStopSignal);
    }
    #endregion

    #region Resist
    private Action<BrainBitResistData> _resistReceived = null;
    public void StartResist(Action<BrainBitResistData> resistReceived){
        _resistReceived = resistReceived;
        sensor.EventBrainBitResistDataRecived += Sensor_EventBrainBitResistDataRecived;

        executeCommand(SensorCommand.CommandStartResist);
    }

    private void Sensor_EventBrainBitResistDataRecived(ISensor sensor, BrainBitResistData data)
    {
        _resistReceived?.Invoke(data);
    }

    public void StopResist()
    {
        sensor.EventBrainBitResistDataRecived -= Sensor_EventBrainBitResistDataRecived;
        executeCommand(SensorCommand.CommandStopResist);
    }
    #endregion

    private void executeCommand(SensorCommand command) {
        try
        {
            if (sensor?.IsSupportedCommand(command) == true)
            {
                sensor?.ExecCommand(command);
            }
        }
        catch (Exception ex) {
            
        }
    }

}
