using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;


namespace CarHoldingService.Configurations
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Title = "Car Holding Microservice API",
            Description = "Car Holding Microservice APIs",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Name = "Microservices Carholding",
                Url = new System.Uri("https://www.carholding.com.br")
            },
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;
    }
}
