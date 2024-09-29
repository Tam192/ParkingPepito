using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FunctionApp.Functions
{
    public class GetResidentDebtReportFunction(ILogger<GetResidentDebtReportFunction> logger, IGetResidentDebtReportUseCase getResidentDebtReportUseCase)
    {
        private readonly ILogger<GetResidentDebtReportFunction> _logger = logger;
        private readonly IGetResidentDebtReportUseCase _getResidentReportUseCase = getResidentDebtReportUseCase ?? throw new ArgumentNullException(nameof(getResidentDebtReportUseCase));

        [Function(nameof(GetResidentDebtReportFunction))]
        [OpenApiOperation(operationId: "GetResidentDebtReportFunction", tags: ["Report"], Summary = "Getting the resident debts report", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/octet-stream", bodyType: typeof(FileContentResult))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogDebug("GetResidentDebtReportFunction - Retrieving the residents debts report.");

            //1. Creamos la respuesta y seteamos los headers necesarios
            HttpResponseData response = req.CreateResponse();
            response.Headers.Add("Content-Type", "application/octet-stream");

            //2. Llamamos al usecase para ejecutar la lógica de negocio, obtener los bytes y añadirlos a la respuesta
            byte[] bytes = await _getResidentReportUseCase.GetResidentDebtReportsAsync();
            await response.WriteBytesAsync(bytes);

            //3. Devolvemos el resultado.
            return response;
        }
    }
}
