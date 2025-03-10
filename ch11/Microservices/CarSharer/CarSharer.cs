using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CarSharer
{
    public class CarSharer
    {
        private readonly ILogger<CarSharer> _logger;

        public CarSharer(ILogger<CarSharer> logger)
        {
            _logger = logger;
        }

        [Function("Create")]
        [OpenApiOperation("Create", "Creates a CarSharer data")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/Create")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a post request.");
            return new OkObjectResult("C# HTTP trigger function processed a post request");
        }

        [Function("Read")]
        [OpenApiOperation("Read", "Reads a CarSharer data")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Read([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/Read")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a get request.");
            return new OkObjectResult("C# HTTP trigger function processed a get request.");
        }


        [Function("Update")]
        [OpenApiOperation("Update", "Updates a CarSharer data")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Update([HttpTrigger(AuthorizationLevel.Function, "put", Route = "v1/Update")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed an update request.");
            return new OkObjectResult("C# HTTP trigger function processed an update request.");
        }

        [Function("Delete")]
        [OpenApiOperation("Delete", "Deletes a CarSharer data")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v1/Delete")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }
    }
}
