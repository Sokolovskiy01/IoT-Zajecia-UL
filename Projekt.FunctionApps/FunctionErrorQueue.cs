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
    public static class FunctionErrorQueue
    {

        [FunctionName("FunctionErrorQueue")]
        public static async Task Run([ServiceBusTrigger("%ServiceBusErrorQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ILogger log, ExecutionContext context)
        {
            var messageBody = JsonConvert.DeserializeObject<EmergencyStopErrorMessage>(Encoding.UTF8.GetString(message.Body));
            Console.WriteLine("Recieved message: {0}", message.Body);
            //Console.WriteLine("Converted message: {0}, {1}", messageBody.deviceId, messageBody.time);
            //Console.WriteLine(Resources.IoTHubConnectiontring);

            // execute emergency stop on deviceId in payload
            ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(Resources.IoTHubConnectiontring);

            Console.WriteLine("Emergency stop call result:");
            CloudToDeviceMethod emergencyStopMethod = new CloudToDeviceMethod("EmergencyStop");
            emergencyStopMethod.ResponseTimeout = TimeSpan.FromSeconds(20);
            CloudToDeviceMethodResult emergencyStopMethodResult = await serviceClient.InvokeDeviceMethodAsync(messageBody.deviceId, emergencyStopMethod);
            Console.WriteLine(emergencyStopMethodResult.Status);
            Console.WriteLine(emergencyStopMethodResult.GetPayloadAsJson());
        }

        class EmergencyStopErrorMessage
        {
            public string deviceId { get; set; }
            public DateTime time { get; set; }
        }

    }
}
