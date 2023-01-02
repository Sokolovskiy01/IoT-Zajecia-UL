using Opc.UaFx;
using Opc.UaFx.Client;
using Projekt.Properties;
using Projekt.VirtualDevice;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices;


class Program
{

    private static async Task Main(string[] args)
    {

        using (var opcClient = new OpcClient(Resources.opcServerAddress))
        {
            try
            {
                opcClient.Connect();

                OpcNodeInfo serverNode = opcClient.BrowseNode(OpcObjectTypes.ObjectsFolder);



                // Zakładamy, że mamy 6 urządzeń na produkcji i na iot Hub
                //Connect IoT Hub devices to Opc Client
                Dictionary<string, string> OpcToIoTHubIds = new Dictionary<string, string>();
                OpcToIoTHubIds.Add("ns=2;s=Device 1", "Device_1");
                OpcToIoTHubIds.Add("ns=2;s=Device 2", "Device_2");
                OpcToIoTHubIds.Add("ns=2;s=Device 3", "Device_3");
                OpcToIoTHubIds.Add("ns=2;s=Device 4", "Device_4");
                OpcToIoTHubIds.Add("ns=2;s=Device 5", "Device_5");
                OpcToIoTHubIds.Add("ns=2;s=Device 6", "Device_6");

                Dictionary<VirtualDevice, OpcDeviceData> iotHubDeviceToOpcDeviceData = new Dictionary<VirtualDevice, OpcDeviceData>();
                foreach (var opcDeviceId in OpcToIoTHubIds)
                {
                    DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(Resources.iotDeviceConnectionString, opcDeviceId.Value);
                    await deviceClient.OpenAsync();
                    VirtualDevice vDevice = new VirtualDevice(deviceClient, opcClient);
                    OpcDeviceData deviceData = new OpcDeviceData(opcDeviceId.Key);

                    readOpcDeviceData(opcClient, deviceData);

                    iotHubDeviceToOpcDeviceData.Add(vDevice, deviceData);


                }



                //Read each device data and connect to device twin
                //foreach (var )



                // search for devices
                /*foreach (var device in serverNode.Children())
                {
                    string nodeName = device.DisplayName.ToString();
                    Console.WriteLine(device.NodeId);
                    if (nodeName.Contains("Device"))
                    {
                        // Device found
                        //devicesList.Add(client.BrowseNode(device.NodeId));

                        //OpcDeviceData newDevice = new OpcDeviceData(device.NodeId);
                        OpcValue ProductionStatus = opcClient.ReadNode(device.NodeId + "/ProductionStatus");

                        //deviceDatas.Add(newDevice);
                    }
                }

                while(true)
                {
                    Console.WriteLine('.');



                    await Task.Delay(10000);
                }*/

                opcClient.Disconnect();
            }
            catch (OpcException opcex)
            {
                Console.WriteLine(opcex.Message);
            }
        }


    }

    private static void readOpcDeviceData(OpcClient opcClient, OpcDeviceData deviceData)
    {
        deviceData.ProductionStatus = (int)opcClient.ReadNode(deviceData.nodeId + "/ProductionStatus").Value;
        deviceData.WorkorderId = (string)opcClient.ReadNode(deviceData.nodeId + "/WorkorderId").Value;
        deviceData.ProductionRate = (int)opcClient.ReadNode(deviceData.nodeId + "/ProductionRate").Value;
        deviceData.GoodCount = (long)opcClient.ReadNode(deviceData.nodeId + "/GoodCount").Value;
        deviceData.BadCount = (long)opcClient.ReadNode(deviceData.nodeId + "/BadCount").Value;
        deviceData.Temperature = (double)opcClient.ReadNode(deviceData.nodeId + "/Temperature").Value;
        deviceData.DeviceError = (int)opcClient.ReadNode(deviceData.nodeId + "/DeviceError").Value;
    }

}

