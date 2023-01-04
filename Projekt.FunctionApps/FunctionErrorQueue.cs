using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Projekt.FunctionApps
{
    public static class FunctionErrorQueue
    {

        static int MessageCount = 1;

        [FunctionName("FunctionErrorQueue")]
        public static void Run([ServiceBusTrigger("%ServiceBusErrorQueue%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions, ILogger log, ExecutionContext context)
        {
            //message.EnqueuedTime;
            Console.WriteLine(message.Body);
            Console.WriteLine(MessageCount);
            MessageCount++;
        }

    }
}
