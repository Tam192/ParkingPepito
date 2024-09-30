using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using Application.UseCases;
using FluentValidation;
using FluentValidation.Results;
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
    public class StartMonthFunction(ILogger<StartMonthFunction> logger, IStartMonthUseCase startMonthUseCase, IValidator<StartMonthUseCaseDto> validator)
    {
        private readonly ILogger<StartMonthFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IStartMonthUseCase _startMonthUseCase = startMonthUseCase ?? throw new ArgumentNullException(nameof(startMonthUseCase));
        private readonly IValidator<StartMonthUseCaseDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        [Function(nameof(StartMonthFunction))]
        [OpenApiOperation(operationId: "StartMonthFunction", tags: ["Start Month"], Summary = "Start Month", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(StartMonthUseCaseDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BaseResponse<bool>), Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            BaseResponse<bool>? resp;

            StartMonthUseCaseDto? input = await req.ReadFromJsonAsync<StartMonthUseCaseDto>();

            if (input is null)
            {
                resp = new BaseResponse<bool>
                {
                    Errors = ["Error - Body Required"],
                    Results = [],
                    Total = 0
                };
                return new BadRequestObjectResult(resp);
            }

            ValidationResult validationResult = await _validator.ValidateAsync(input);
            if (!validationResult.IsValid)
            {
                resp = new BaseResponse<bool>
                {
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList(),
                    Results = [],
                    Total = 0
                };

                return new BadRequestObjectResult(resp);
            }

            _logger.LogDebug("StartMonthFunction - Starting month...");

            //2. Llamamos al usecase para ejecutar la lógica de negocio
            resp = await _startMonthUseCase.StartMonthAsync(input);

            //3. Devolvemos el resultado.
            return resp.Errors.Count > 0 ? new BadRequestObjectResult(resp) : new OkObjectResult(resp);
        }
    }
}
