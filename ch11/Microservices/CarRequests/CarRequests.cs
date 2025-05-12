using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using SharedMessages.BasicTypes;
using SharedMessages.RouteNegotiation;
using System.Net;
using System.Text.Json;

namespace CarRequests
{
    public class CarRequests
    {
        private readonly ILogger<CarRequests> _logger;

        public CarRequests(ILogger<CarRequests> logger)
        {
            _logger = logger;
        }

        [Function("Add")]
        [OpenApiOperation("Add", "Inserts a ride request with source, destination, and date. It is important to have confirmation if the request has been registered or not.")]
        [OpenApiRequestBody("application/json", typeof(RouteRequestMessage), Description = "The request body containing the ride request details.")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public async Task<IActionResult> Add([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/Add")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a post request.");
            // Ler o corpo da requisição
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Desserializar o JSON para o objeto RouteRequestMessage
            var routeRequest = JsonSerializer.Deserialize<RouteRequestMessage>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Read and deserialize the request body into a RouteRequestMessage object
            if (SampleDB.SampleDatabase.Instance.AddRequest(routeRequest))
                return new OkObjectResult("C# HTTP trigger function processed a post request");
            else
                return new ConflictObjectResult("C# HTTP trigger function processed a post request with an id already taken");
        }

        [Function("GetMyRequests")]
        [OpenApiOperation("GetMyRequests", "Lists active requests with matching car-sharer options. Matching routes contain also the car owner that can be used to get user information from the authentication server.")]
        [OpenApiRequestBody("application/json", typeof(AuthorizedUserMessage), Description = "The request body containing the ride request details.")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(IActionResult))]
        [OpenApiResponseWithBody(HttpStatusCode.Unauthorized, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.NotFound, "text/plain", typeof(string))]
        [OpenApiResponseWithBody(HttpStatusCode.InternalServerError, "text/plain", typeof(string))]
        public async Task<IActionResult> GetMyRequests([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/GetMyRequests")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a get request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Desserializar o JSON para o objeto RouteRequestMessage
            var authorizedUser = JsonSerializer.Deserialize<AuthorizedUserMessage>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


            if (authorizedUser == null)
            {
                return new BadRequestObjectResult("User data cannot be null.");
            }

            if (DateTime.Now > authorizedUser.ValidTo)
            {
                return new UnauthorizedObjectResult("Token expired."); // Return unauthorized if invalid
            }

            List< RouteRequestMessage> requests = SampleDB.SampleDatabase.Instance.GetMyRequests(authorizedUser.UserId);

            return new OkObjectResult(requests);
        }

    }
}
