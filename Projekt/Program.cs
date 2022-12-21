using Opc.UaFx;
using Opc.UaFx.Client;

List<DeviceData> deviceDatas = new List<DeviceData>();

using (var client = new OpcClient("opc.tcp://localhost:4840/"))
{
    try
    {
        client.Connect();

        // we will have 


        OpcNodeInfo serverNodes = client.BrowseNode(OpcObjectTypes.ObjectsFolder);

        foreach (var device in serverNodes.Children())
        {
            string nodeName = device.DisplayName.ToString();
            if (nodeName.Contains("Device"))
            {
                // Device found
                //devicesList.Add(client.BrowseNode(device.NodeId));
                
                DeviceData newDevice = new DeviceData(device.NodeId);
                OpcValue ProductionStatus = client.ReadNode(device.NodeId + "/ProductionStatus");

                deviceDatas.Add(newDevice);
            }
        }

        while(true)
        {
            Console.WriteLine('.');



            Thread.Sleep(5000);
        }

        client.Disconnect();
    }
    catch (OpcException opcex)
    {
        Console.WriteLine(opcex.Message);
    }
}