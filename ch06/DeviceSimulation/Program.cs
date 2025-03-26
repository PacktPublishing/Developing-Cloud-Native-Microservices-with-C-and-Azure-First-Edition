using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

class Program
{
    private static string connectionStringDevice0001 = "[connection-string-for-device0001]";
    private static string connectionStringDevice0002 = "[connection-string-for-device0002]";
    

    static async Task Main()
    {
        await SimulateDeviceAsync(connectionStringDevice0001, "Car 0001 - Hello Message!");
        await SimulateDeviceAsync(connectionStringDevice0002, "Car 0002 - Hello Message!");
        Console.WriteLine("Messages sent!");
    }

    /// <summary>
    /// Simulates a device by creating a DeviceClient and sending a message.
    /// </summary>
    /// <param name="connectionString">The connection string of the device.</param>
    /// <param name="message">The message to be sent by the device.</param>
    private static async Task SimulateDeviceAsync(string connectionString, string message)
    {
        var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
        await SendMessageAsync(deviceClient, message);
    }

    /// <summary>
    /// Sends a message to the IoT hub using the provided DeviceClient.
    /// </summary>
    /// <param name="deviceClient">The DeviceClient used to send the message.</param>
    /// <param name="message">The message to be sent.</param>
    private static async Task SendMessageAsync(DeviceClient deviceClient, string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var iotMessage = new Message(messageBytes);

        await deviceClient.SendEventAsync(iotMessage);
    }
}
