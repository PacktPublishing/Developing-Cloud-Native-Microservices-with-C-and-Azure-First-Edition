using System;
using System.Text.Json;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SharedMessages.VehicleTracking;

namespace VehicleTrackingFunction
{
    /// <summary>
    /// Azure Function to process vehicle tracking messages from Event Hub.
    /// </summary>
    public class VehicleTracking
    {
        private readonly ILogger<VehicleTracking> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleTracking"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public VehicleTracking(ILogger<VehicleTracking> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Function triggered by Event Hub messages.
        /// </summary>
        /// <param name="events">Array of EventData received from Event Hub.</param>
        [Function(nameof(VehicleTracking))]
        public async Task Run([EventHubTrigger("messages/events", Connection = "CarSharingIoTEventHub")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                var jsonString = @event.EventBody.ToString();
                if (!string.IsNullOrEmpty(jsonString))
                {
                    VehicleTrackingMessage? vehicleTrackingMessage = JsonSerializer.Deserialize<VehicleTrackingMessage>(jsonString);
                    if (vehicleTrackingMessage != null)
                    {
                        await SaveDataToDatabase(vehicleTrackingMessage);
                        await AlertDataToRabbitMQ(vehicleTrackingMessage);
                    }
                }
            }
        }

        /// <summary>
        /// Sends vehicle tracking data to RabbitMQ.
        /// </summary>
        /// <param name="vehicleTrackingMessage">The vehicle tracking message.</param>
        private async Task AlertDataToRabbitMQ(VehicleTrackingMessage vehicleTrackingMessage)
        {
            // Implementation for alerting data to RabbitMQ
            Console.WriteLine($"Vehicle tracking data alerted to RabbitMQ: ID = {vehicleTrackingMessage.VehicleId}; Speed = {vehicleTrackingMessage.Speed}");
        }

        /// <summary>
        /// Saves vehicle tracking data to CosmosDB database.
        /// </summary>
        /// <param name="vehicleTrackingMessage">The vehicle tracking message.</param>
        private async Task SaveDataToDatabase(VehicleTrackingMessage vehicleTrackingMessage)
        {
            // Implementation for saving data to the database CosmosDB
            Console.WriteLine($"Vehicle tracking data saved to database: ID = {vehicleTrackingMessage.VehicleId}; Speed = {vehicleTrackingMessage.Speed}");
        }
    }
}
