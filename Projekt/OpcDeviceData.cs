using Opc.UaFx;

class OpcDeviceData
{
    public OpcNodeId nodeId { get; }

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
    public DateTime lastMaitananceDate { get; set; }
    public DateTime lastErrorDate { get; set; }

    public OpcDeviceData(OpcNodeId nodeId)
    {
        this.nodeId = nodeId;
    }

    public string getTelemetryJSON(string ioTDeviceId)
    {
        return "{\"opc_device_id\":\"" + nodeId.ToString() + "\"," +
            "\"iot_device_id\":\"" + ioTDeviceId + "\"," +
            "\"production_status\":" + ProductionStatus + "," +
            "\"workorder_id\":\"" + WorkorderId + "\"," +
            "\"good_count\":" + GoodCount + "," +
            "\"bad_count\":" + BadCount + "," +
            "\"temperature\":" + Temperature.ToString().Replace(',','.') + "}";
    }

}