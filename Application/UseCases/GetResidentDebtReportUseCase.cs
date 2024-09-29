using Application.Dtos.Views;
using AutoMapper;
using Core.Interfaces.Repository;
using Core.Views;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Application.UseCases
{
    public interface IGetResidentDebtReportUseCase
    {
        public Task<byte[]> GetResidentDebtReportsAsync();
    }

    public class GetResidentDebtReportUseCase(ILogger<GetResidentDebtReportUseCase> logger, IViewsRepository<ResidentDebts> residentDebtsRepository, IMapper mapper) : IGetResidentDebtReportUseCase
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IViewsRepository<ResidentDebts> _residentDebtsRepository = residentDebtsRepository ?? throw new ArgumentNullException(nameof(residentDebtsRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<byte[]> GetResidentDebtReportsAsync()
        {
            //1. Obtenemos los datos de la vista
            _logger.LogDebug("Getting the resident debts report data.");
            List<ResidentDebts> residentDebts = await _residentDebtsRepository.GetAllAsync(null, order => order.OrderBy(x => x.VehicleId));

            //2. Los mapeamos a DTO para no trabajar directamente con las entidades.
            List<ResidentDebtsDto> residentDebtsDtos = residentDebts.Select(_mapper.Map<ResidentDebtsDto>).ToList();

            //3. Con los datos de la lista de dtos, creamos el fichero.
            _logger.LogDebug("Generating the resident debts report file.");

            return GetCsvByteArray(residentDebtsDtos);
        }

        private static byte[] GetCsvByteArray(IEnumerable<ResidentDebtsDto> dtos)
        {
            CsvConfiguration csvConfig = CsvConfiguration.FromAttributes<ResidentDebtsDto>(
                CultureInfo.InvariantCulture
                );
            using MemoryStream memoryStream = new();
            using StreamWriter streamWriter = new(memoryStream);
            using CsvWriter csvWriter = new(streamWriter, csvConfig);

            csvWriter.WriteRecords(dtos);
            streamWriter.Flush();

            return memoryStream.ToArray();
        }
    }
}
