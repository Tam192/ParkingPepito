using Application.Dtos;
using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public interface IStartMonthUseCase
    {
        public Task<BaseResponse<bool>> StartMonthAsync(StartMonthUseCaseDto request);
    }

    public class StartMonthUseCase(ILogger<StartMonthUseCase> logger, IEntitiesRepository<Stay> stayRepository, IMapper mapper) : IStartMonthUseCase
    {
        private readonly ILogger<StartMonthUseCase> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEntitiesRepository<Stay> _stayRepository = stayRepository ?? throw new ArgumentNullException(nameof(stayRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        public async Task<BaseResponse<bool>> StartMonthAsync(StartMonthUseCaseDto request)
        {
            BaseResponse<bool> resp = new();

            try
            {
                //1. Buscamos todas las estancias de vehículos que no sean "No Residentes"
                _logger.LogDebug("Searching all the current stays for official and resident vehicles.");
                List<Stay> resultList = await _stayRepository.GetAllAsync(stay => 
                    stay.Vehicle.VehicleTypeId != 3 && (stay.DeleteEmployee == null && stay.DeleteDate == null), stay => stay.OrderBy(x => x.Id), "Vehicle");

                //2. Todas las estancias de los vehículos oficiales las eliminamos.
                foreach (Stay stay in resultList.Where(x => x.Vehicle.VehicleTypeId == 1))
                {
                    Stay updatedStay = _mapper.Map(request, stay);

                    //Hacemos un borrado logico
                    _ = await _stayRepository.UpdateAsync(updatedStay);
                }

                //3. Todas las estancias de los vehiculos residentes las cerramos
                foreach(Stay stay in resultList.Where(x => x.Vehicle.VehicleTypeId == 2))
                {
                    stay.FinalDate = DateTime.Now;

                    await _stayRepository.UpdateAsync(stay);
                }

                //5. Guardamos todos los cambios
                _ = await _stayRepository.SaveAsync();

                resp.Results.Add(true);
                resp.Total = resultList.Count;
            }
            catch (Exception ex)
            {
                resp.Errors.Add(ex.ToString());
            }

            return resp;
        }
    }
}
