using Opc.UaFx;

class OpcDeviceData
{
    OpcNodeId nodeId { get; }

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

    public OpcDeviceData(OpcNodeId nodeId)
    {
        this.nodeId = nodeId;
    }

}