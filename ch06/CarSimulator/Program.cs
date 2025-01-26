using System.Text;
using System.Text.Json;
using Microsoft.Azure.Devices.Client;
using SharedMessages.BasicTypes;
using SharedMessages.VehicleTracking;

/// <summary>
/// The main class for the Car Simulator program.
/// </summary>
class Program
{
    /// <summary>
    /// The connection string for the car device.
    /// </summary>
    private static string carConnectionString = "[device connection string]";

    /// <summary>
    /// The main entry point for the program.
    /// </summary>
    static async Task Main()
    {
        while (true)
        {
            // Create a new vehicle tracking message with random data
            VehicleTrackingMessage vehicleTrackingMessage = new VehicleTrackingMessage
            {
                VehicleId = Guid.NewGuid(),
                Location = new GeoLocalizationMessage
                {
                    Latitude = 47.6426,
                    Longitude = -122.1301
                },
                Speed = 60 + DateTime.Now.Second,
                CarStatus = 1,
                BatteryLevel = 100 - DateTime.Now.Second,
                FuelLevel = 100,
                TirePressure = 32
            };

            // Simulate sending the device message
            await SimulateDeviceAsync(carConnectionString, vehicleTrackingMessage);
            Console.WriteLine("Vehicle tracking sent!");
            await Task.Delay(new Random().Next(10000, 20000));
        }
    }

    /// <summary>
    /// Simulates sending a device message to the IoT hub.
    /// </summary>
    /// <param name="connectionString">The connection string for the device.</param>
    /// <param name="message">The vehicle tracking message to send.</param>
    private static async Task SimulateDeviceAsync(string connectionString, VehicleTrackingMessage message)
    {
        var deviceClient = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
        string jsonMessage = JsonSerializer.Serialize(message);
        await SendMessageAsync(deviceClient, jsonMessage);
    }

    /// <summary>
    /// Sends a message to the IoT hub.
    /// </summary>
    /// <param name="deviceClient">The device client to use for sending the message.</param>
    /// <param name="message">The message to send.</param>
    private static async Task SendMessageAsync(DeviceClient deviceClient, string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var iotMessage = new Message(messageBytes);

        await deviceClient.SendEventAsync(iotMessage);
    }
}
