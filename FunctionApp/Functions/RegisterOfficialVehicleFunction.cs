using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;
using Application.UseCases;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace FunctionApp.Functions
{
    public class RegisterOfficialVehicleFunction(ILogger<RegisterOfficialVehicleFunction> logger, IRegisterOfficialVehicleUseCase registerOfficialVehicleUseCase, IValidator<RegisterOfficialVehicleDto> validator)
    {
        private readonly ILogger<RegisterOfficialVehicleFunction> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRegisterOfficialVehicleUseCase _registerOfficialVehicleUseCase = registerOfficialVehicleUseCase ?? throw new ArgumentNullException(nameof(registerOfficialVehicleUseCase));
        private readonly IValidator<RegisterOfficialVehicleDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));


        [Function(nameof(RegisterOfficialVehicleFunction))]
        [OpenApiOperation(operationId: "RegisterOfficialVehicleFunction", tags: ["Register"], Summary = "Register official vehicle", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(RegisterOfficialVehicleDto), Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BaseResponse<VehicleDto>), Description = "The OK response message containing a JSON result.")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            BaseResponse<VehicleDto>? resp;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<RegisterOfficialVehicleDto>(requestBody);

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

            var validationResult = await _validator.ValidateAsync(input);
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

            _logger.LogDebug("RegisterOfficialVehicleFunction - Registering an official vehicle with plat number {0}", input.PlateNumber);

            //2. Llamamos al usecase para ejecutar la lógica de negocio
            resp = await _registerOfficialVehicleUseCase.RegisterOfficialVehicleAsync(input);

            //3. Devolvemos el resultado.
            return resp.Errors.Count > 0 ? new BadRequestObjectResult(resp) : new OkObjectResult(resp);
        }
    }
}
