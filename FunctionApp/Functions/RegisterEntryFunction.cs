using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using Application.UseCases;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionApp.Functions
{
    public class RegisterEntryFunction(ILogger<RegisterEntryFunction> logger, IRegisterEntryUseCase registerEntryUseCase, IValidator<RegisterEntryUseCaseDto> validator)
    {
        private readonly ILogger<RegisterEntryFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRegisterEntryUseCase _registerEntryUseCase = registerEntryUseCase ?? throw new ArgumentNullException(nameof(registerEntryUseCase));
        private readonly IValidator<RegisterEntryUseCaseDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        [Function(nameof(RegisterEntryFunction))]
        [OpenApiOperation(operationId: "RegisterEntryFunction", tags: ["Register"], Summary = "Register vehicle entry", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RegisterEntryUseCaseDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BaseResponse<StayDto>), Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            BaseResponse<StayDto>? resp;

            var input = await req.ReadFromJsonAsync<RegisterEntryUseCaseDto>();

            if (input is null)
            {
                resp = new BaseResponse<StayDto>
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
                resp = new BaseResponse<StayDto>
                {
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList(),
                    Results = [],
                    Total = 0
                };

                return new BadRequestObjectResult(resp);
            }

            _logger.LogDebug("RegisterEntryFunction - Registering an entry for vehicle with plat number {0}", input.PlateNumber);

            //2. Llamamos al usecase para ejecutar la lógica de negocio
            resp = await _registerEntryUseCase.RegisterEntryAsync(input);

            //3. Devolvemos el resultado.
            return resp.Errors.Count > 0 ? new BadRequestObjectResult(resp) : new OkObjectResult(resp);
        }
    }
}
