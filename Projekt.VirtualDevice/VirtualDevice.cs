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

    }
}