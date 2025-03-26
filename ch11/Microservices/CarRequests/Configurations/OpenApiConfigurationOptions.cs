using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;


namespace CarRequests.Configurations
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Title = "Car Requests Microservice API",
            Description = "Car Requests Microservice APIs",
            Version = "v1",
            Contact = new OpenApiContact
            {
                Name = "Microservices Car Requests",
                Url = new System.Uri("https://www.carrequests.com.br")
            },
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;
    }
}
