using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuditService
{
    public class Audit
    {
        private readonly ILogger _logger;

        public Audit(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Audit>();
        }

        [Function("Audit")]
        public void Run(
            [SqlTrigger("[dbo].[Carholder]", "CarShareConnectionString")] IReadOnlyList<SqlChange<Carholder>> changes,
                FunctionContext context)
        {
            _logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

        }
    }

    public class Carholder
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
