using Opc.UaFx;

class OpcDeviceData
{
    public OpcNodeId nodeId { get; }
    public String IoTDeviceId { get; }

    public Int32 ProductionStatus { get; set; } = 0;
    public String WorkorderId { get; set; } = string.Empty;

    private Int32 _productionRate;
    public Int32 ProductionRate { get { return _productionRate; }
        set {
            if (value >= 0 && value <= 100) _productionRate = value;
            else throw new ArgumentOutOfRangeException();
        }
    }
    public Int64 GoodCount { get; set; } = 0;
    public Int64 BadCount { get; set; } = 0;
    public Double Temperature { get; set; } = 0.0;
    public Int32 DeviceError { get; set; } = 0;
    public DateTime LastMaitananceDate { get; set; }
    public DateTime LastErrorDate { get; set; }

    public OpcDeviceData(OpcNodeId nodeId, string IoTDeviceId)
    {
        this.nodeId = nodeId;
        this.IoTDeviceId = IoTDeviceId;
    }

    public string getTelemetryJSON()
    {
        return "{\"opc_device_id\":\"" + nodeId.ToString() + "\"," +
            "\"iot_device_id\":\"" + IoTDeviceId + "\"," +
            "\"production_status\":" + ProductionStatus + "," +
            "\"workorder_id\":\"" + WorkorderId + "\"," +
            "\"good_count\":" + GoodCount + "," +
            "\"bad_count\":" + BadCount + "," +
            "\"temperature\":" + Temperature.ToString().Replace(',','.') + "}";
    }

    public string getErrorsJSON()
    {
        string errorString = Convert.ToString(DeviceError, 2).PadLeft(4, '0');
        // [U,S,P,E]
        return "{\"opc_device_id\":\"" + nodeId.ToString() + "\"," +
            "\"iot_device_id\":\"" + IoTDeviceId + "\"," +
            "\"is_error\":" + ((DeviceError > 0) ? "true" : "false") + "," +
            "\"error_unknown\":" + ((errorString[0] == '1') ? "true" : "false") + "," +
            "\"error_sensor\":" + ((errorString[1] == '1') ? "true" : "false") + "," +
            "\"error_power\":" + ((errorString[2] == '1') ? "true" : "false") + "," +
            "\"error_emergency_stop\":" + ((errorString[3] == '1') ? "true" : "false") + "}";
    }

}