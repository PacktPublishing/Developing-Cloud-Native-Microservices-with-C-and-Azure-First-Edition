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
        [OpenApiOperation("Create", "Creates a new route with date and all towns’ milestones")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Create([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/Create")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a post request.");
            return new OkObjectResult("C# HTTP trigger function processed a post request");
        }


        [Function("Delete")]
        [OpenApiOperation("Delete", "Removes a specific route")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Delete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "v1/Delete")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }

        [Function("Close")]
        [OpenApiOperation("Close", "Closes a route to prevent further matching")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Close([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/Close")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }

        [Function("Extend")]
        [OpenApiOperation("Extend", "Accepts user requests into an existing route")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult Extend([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/Extend")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }

        [Function("GetSuggestedExtensions")]
        [OpenApiOperation("GetSuggestedExtensions", "Lists compatible ride requests for a route")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult GetSuggestedExtensions([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/GetSuggestedExtensions")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }


        [Function("GetActiveRoutes")]
        [OpenApiOperation("GetActiveRoutes", "Lists all active (not expired or deleted) routes for a specific user.")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public IActionResult GetActiveRoutes([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/GetActiveRoutes")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a delete request.");
            return new OkObjectResult("C# HTTP trigger function processed a delete request.");
        }
    }
}
