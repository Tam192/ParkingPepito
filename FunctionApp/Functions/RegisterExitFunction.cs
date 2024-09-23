using Application.Dtos;
using Application.Dtos.UseCases;
using Application.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionApp.Functions
{
    public class RegisterExitFunction(ILogger<RegisterExitFunction> logger, IRegisterExitUseCase registerExitUse, IValidator<RegisterExitUseCaseDto> validator)
    {
        private readonly ILogger<RegisterExitFunction> _logger = logger;
        private readonly IRegisterExitUseCase _registerEntryUseCase = registerExitUse ?? throw new ArgumentNullException(nameof(registerExitUse));
        private readonly IValidator<RegisterExitUseCaseDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        [Function("RegisterExitFunction")]
        [OpenApiOperation(operationId: "RegisterExitFunction", tags: ["Register Exit"], Summary = "Register vehicle exit", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RegisterExitUseCaseDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BaseResponse<decimal>), Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            BaseResponse<decimal>? resp;

            var input = await req.ReadFromJsonAsync<RegisterExitUseCaseDto>();

            if (input is null)
            {
                resp = new BaseResponse<decimal>
                {
                    Errors = ["Error - Body Required"],
                    Results = [],
                    Total = 0
                };
                return new BadRequestObjectResult(resp);
            }

            var validationResult = await _validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                resp = new BaseResponse<decimal>
                {
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList(),
                    Results = [],
                    Total = 0
                };

                return new BadRequestObjectResult(resp);
            }

            _logger.LogDebug("RegisterExitFunction - Registering an exit for vehicle with plat number {0}", input.PlateNumber);

            //2. Llamamos al usecase para ejecutar la lógica de negocio
            resp = await _registerEntryUseCase.RegisterExitAsync(input);

            //3. Devolvemos el resultado.
            return resp.Errors.Count > 0 ? new BadRequestObjectResult(resp) : new OkObjectResult(resp);
        }
    }
}
