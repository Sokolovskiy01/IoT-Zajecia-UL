using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Opc.UaFx;
using Opc.UaFx.Client;
using System.Text;
using System.Net.Mime;

namespace Projekt.VirtualDevice
{
    public class VirtualDevice
    {
        private readonly DeviceClient _deviceClient;
        private readonly OpcClient _opcClient;

        public VirtualDevice(DeviceClient deviceClient, OpcClient opcClient)
        {
            _deviceClient = deviceClient;
            _opcClient = opcClient;
        }

        #region Send Message

        public async Task SendMessage(string messageData)
        {
            if (messageData != null)
            {
                Message message = new Message(Encoding.UTF8.GetBytes(messageData));
                message.ContentType = MediaTypeNames.Application.Json;
                message.ContentEncoding = "utf-8";
                Console.WriteLine($"\t{DateTime.Now.ToLocalTime()}> D2C Sending message: {messageData}");

                await _deviceClient.SendEventAsync(message);
                Console.WriteLine();
            }
        }

        #endregion Send Message


        #region Recieve Message

        private async Task OnC2dMessageReceivedAsync(Message receivedMessage, object _)
        {
            Console.WriteLine($"\t{DateTime.Now}> C2D message callback - message received with Id={receivedMessage.MessageId}.");
            PrintMessage(receivedMessage);

            await _deviceClient.CompleteAsync(receivedMessage);
            Console.WriteLine($"\t{DateTime.Now}> Completed C2D message with Id={receivedMessage.MessageId}.");

            receivedMessage.Dispose();
        }

        private void PrintMessage(Message receivedMessage)
        {
            string messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
            Console.WriteLine($"\t\tReceived message: {messageData}");

            int propCount = 0;
            foreach (var prop in receivedMessage.Properties)
            {
                Console.WriteLine($"\t\tProperty[{propCount++}> Key={prop.Key} : Value={prop.Value}");
            }
        }

        #endregion Recieve Message

        #region Device Twin Methods

        public async Task SetTwinDataAsync(int deviceError, int productionRate, DateTime lastMaintananceDate, DateTime lastErrorDate)
        {
            var deviceTwin = await _deviceClient.GetTwinAsync();

            TwinCollection reportedProperties = new TwinCollection();
            reportedProperties["device_errors"] = deviceError;
            reportedProperties["production_rate"] = productionRate;
            reportedProperties["last_maintenance_date"] = lastMaintananceDate;
            reportedProperties["last_error_date"] = lastErrorDate;
            
            await  _deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
            Console.WriteLine("Device twin was set...");
        }

        public async Task UpdateTwinErrorDataAsync(int deviceError)
        {
            var deviceTwin = await _deviceClient.GetTwinAsync();
            Console.WriteLine($"{DateTime.Now}> Device Twin value was updated.");

            TwinCollection reportedProperties = new TwinCollection();
            reportedProperties["device_errors"] = deviceError;
            reportedProperties["last_error_date"] = DateTime.Now;

            await _deviceClient.UpdateReportedPropertiesAsync(reportedProperties);
        }

        private async Task OnDesiredProductionRateChanged(TwinCollection desiredProperties, object userContext)
        {
            Console.WriteLine("Device Twin's desired production rate changed, id: " + (string)userContext);
            int newProductionRate = desiredProperties["production_rate"];
            string nodeId = (string)userContext + "/ProductionRate";

            OpcStatus result = _opcClient.WriteNode(nodeId, newProductionRate);
            Console.WriteLine($"\t{DateTime.Now}> opcClient.WriteNode is result good: " + result.IsGood.ToString());
            TwinCollection reportedProperties = new TwinCollection();
            reportedProperties["last_datetime_desired_production_rate_changed"] = DateTime.Now;
            reportedProperties["production_rate"] = newProductionRate;

            await _deviceClient.UpdateReportedPropertiesAsync(reportedProperties).ConfigureAwait(false);
        }

        #endregion Device Twin Methods

    }
}