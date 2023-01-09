using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using Projekt.FunctionApps.Properties;

namespace Projekt.FunctionApps
{
    public static class FunctionDecreaseRateQueue
    {
        [FunctionName("FunctionDecreaseRateQueue")]
        public static async Task Run([ServiceBusTrigger("%ServiceBusDecreaseRateQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ILogger log, ExecutionContext context)
        {
            var messageBody = JsonConvert.DeserializeObject<DecreaseRateMessage>(Encoding.UTF8.GetString(message.Body));
            log.LogInformation($"Recieved decrease production rate message: {message.Body}");

            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Resources.IoTHubConnectiontring);

            log.LogInformation("DecreaseProductRate call result:");
            CloudToDeviceMethod emergencyStopMethod = new CloudToDeviceMethod("DecreaseProductRate");
            emergencyStopMethod.ResponseTimeout = TimeSpan.FromSeconds(20);
            CloudToDeviceMethodResult emergencyStopMethodResult = await serviceClient.InvokeDeviceMethodAsync(messageBody.deviceId, emergencyStopMethod);
            log.LogInformation(emergencyStopMethodResult.Status.ToString());
            log.LogInformation(emergencyStopMethodResult.GetPayloadAsJson());
        }

        class DecreaseRateMessage
        {
            public string deviceId { get; set; }
            public DateTime time { get; set; }
        }
    }
}
