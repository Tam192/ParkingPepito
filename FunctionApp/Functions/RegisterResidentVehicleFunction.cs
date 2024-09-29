using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using Application.UseCases;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionApp.Functions
{
    public class RegisterResidentVehicleFunction(ILogger<RegisterResidentVehicleFunction> logger, IRegisterResidentVehicleUseCase registerOfficialVehicleUseCase, IValidator<RegisterResidentVehicleDto> validator)
    {
        private readonly ILogger<RegisterResidentVehicleFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRegisterResidentVehicleUseCase _registerResidentVehicleUseCase = registerOfficialVehicleUseCase ?? throw new ArgumentNullException(nameof(registerOfficialVehicleUseCase));
        private readonly IValidator<RegisterResidentVehicleDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        [Function(nameof(RegisterResidentVehicleFunction))]
        [OpenApiOperation(operationId: "RegisterResidentVehicleFunction", tags: ["Register"], Summary = "Register resident vehicle", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RegisterResidentVehicleDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BaseResponse<VehicleDto>), Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            BaseResponse<VehicleDto>? resp;

            RegisterResidentVehicleDto? input = await req.ReadFromJsonAsync<RegisterResidentVehicleDto>();

            if (input is null)
            {
                resp = new BaseResponse<VehicleDto>
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
                resp = new BaseResponse<VehicleDto>
                {
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList(),
                    Results = [],
                    Total = 0
                };

                return new BadRequestObjectResult(resp);
            }

            _logger.LogDebug("RegisterResidentVehicleFunction - Registering an resident vehicle with plat number {0}", input.PlateNumber);

            //2. Llamamos al usecase para ejecutar la lógica de negocio
            resp = await _registerResidentVehicleUseCase.RegisterResidentVehicleAsync(input);

            //3. Devolvemos el resultado.
            return resp.Errors.Count > 0 ? new BadRequestObjectResult(resp) : new OkObjectResult(resp);
        }
    }
}
