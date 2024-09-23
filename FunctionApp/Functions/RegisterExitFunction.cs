using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp.Functions
{
    public class RegisterExitFunction
    {
        private readonly ILogger<RegisterExitFunction> _logger;

        public RegisterExitFunction(ILogger<RegisterExitFunction> logger)
        {
            _logger = logger;
        }

        [Function("RegisterExitFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
