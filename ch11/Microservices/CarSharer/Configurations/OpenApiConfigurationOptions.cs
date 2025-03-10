using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;


namespace CarSharer.Configurations
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Title = "Car Sharer Microservice API",
            Description = "Car Sharer Microservice APIs",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Name = "Microservices Car Sharer",
                Url = new System.Uri("https://www.carsharer.com.br")
            },
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;
    }
}
