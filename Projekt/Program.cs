using Opc.UaFx;
using Opc.UaFx.Client;

KeyValuePair<OpcNodeId, string> OPCDeviceIdToIoTHubDeviceId = new KeyValuePair<OpcNodeId, string>();

using (var client = new OpcClient("opc.tcp://localhost:4840/"))
{
    try
    {
        client.Connect();

        OpcNodeInfo serverNodes = client.BrowseNode(OpcObjectTypes.ObjectsFolder);

        // Zakładamy, że mamy 6 urządzeń na produkcji i na iot Hub

        

        // search for devices
        foreach (var device in serverNodes.Children())
        {
            string nodeName = device.DisplayName.ToString();
            Console.WriteLine(device.NodeId);
            if (nodeName.Contains("Device"))
            {
                // Device found
                //devicesList.Add(client.BrowseNode(device.NodeId));
                
                //OpcDeviceData newDevice = new OpcDeviceData(device.NodeId);
                OpcValue ProductionStatus = client.ReadNode(device.NodeId + "/ProductionStatus");

                //deviceDatas.Add(newDevice);
            }
        }

        /*while(true)
        {
            Console.WriteLine('.');



            Thread.Sleep(5000);
        }*/

        client.Disconnect();
    }
    catch (OpcException opcex)
    {
        Console.WriteLine(opcex.Message);
    }
}