using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public interface IRegisterEntryUseCase
    {
        public Task<BaseResponse<StayDto>> RegisterEntryAsync(RegisterEntryUseCaseDto request);
    }

    public class RegisterEntryUseCase(ILogger<RegisterEntryUseCase> logger, IEntitiesRepository<Vehicle> vehicleRepository, IEntitiesRepository<Stay> stayRepository, IMapper mapper) : IRegisterEntryUseCase
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEntitiesRepository<Vehicle> _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IEntitiesRepository<Stay> _stayRepository = stayRepository ?? throw new ArgumentNullException(nameof(stayRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<BaseResponse<StayDto>> RegisterEntryAsync(RegisterEntryUseCaseDto request)
        {
            BaseResponse<StayDto> resp = new();

            try
            {
                //1. Buscamos en la base de datos si tenemos registrada la matricula. Si no la tenemos, creamos el vehiculo como no residente.
                Vehicle vehicle = await GetOrCreateVehicleAsync(request.PlateNumber);

                //2. Registramos la entrada.
                StayDto dto = await CreateStay(request, vehicle);

                //3. Devolvemos el objeto de la estancia creado.
                resp.Results.Add(dto);
            }
            catch (Exception ex)
            {
                resp.Errors.Add(ex.ToString());
            }

            resp.Total = resp.Results.Count;
            return resp;
        }

        private async Task<Vehicle> GetOrCreateVehicleAsync(string plateNumber)
        {
            _logger.LogDebug("Checking if the plate number enterer is registered.");

            Vehicle? vehicle = await _vehicleRepository.GetAsync(vehicle => vehicle.PlateNumber == plateNumber, vehicle => vehicle.OrderBy(x => x.Id), null);

            if (vehicle == null)
            {
                _logger.LogDebug("Entered plate number is not register. Starting the vehicle registration...");

                Vehicle newVehicle = new() { PlateNumber = plateNumber, VehicleTypeId = 3 };

                _ = await _vehicleRepository.CreateAsync(newVehicle);
                _ = await _vehicleRepository.SaveAsync();

                _logger.LogDebug("Entered plate number is not register. Starting the vehicle registration...");

                vehicle = newVehicle;
            }

            return vehicle;
        }

        private async Task<StayDto> CreateStay(RegisterEntryUseCaseDto request, Vehicle vehicle)
        {
            _logger.LogDebug("Registering stay for Vehicle with plate number {0}", vehicle.PlateNumber);

            Stay stay = _mapper.Map<Stay>(request);
            stay = _mapper.Map(vehicle, stay);

            _ = await _stayRepository.CreateAsync(stay);
            _ = await _stayRepository.SaveAsync();

            _logger.LogDebug("Stay registered.");

            StayDto dto = _mapper.Map<StayDto>(stay);

            return dto;
        }
    }
}
