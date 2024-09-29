using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public interface IRegisterResidentVehicleUseCase
    {
        public Task<BaseResponse<VehicleDto>> RegisterResidentVehicleAsync(RegisterResidentVehicleDto request);
    }

    public class RegisterResidentVehicleUseCase(ILogger<RegisterEntryUseCase> logger, IEntitiesRepository<Vehicle> vehicleRepository, IMapper mapper) : IRegisterResidentVehicleUseCase
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEntitiesRepository<Vehicle> _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<BaseResponse<VehicleDto>> RegisterResidentVehicleAsync(RegisterResidentVehicleDto request)
        {
            BaseResponse<VehicleDto> resp = new();

            try
            {
                //1. Buscamos en la base de datos la matricula por si ya estuviera previamente registrado el vehiculo.
                _logger.LogDebug("Checking if the plate number enterer is registered.");
                Vehicle? vehicle = await _vehicleRepository.GetAsync(vehicle => vehicle.PlateNumber == request.PlateNumber, vehicle => vehicle.OrderBy(x => x.Id), null);

                if (vehicle is null)
                {
                    //2. No tenemos el vehiculo registrado, procedemos a crearlo.
                    _logger.LogDebug("Entered plate number is not register. Starting the vehicle registration...");
                    vehicle = _mapper.Map<Vehicle>(request);

                    _ = await _vehicleRepository.CreateAsync(vehicle);
                    _ = await _vehicleRepository.SaveAsync();
                }
                else
                {
                    //3. Si tenemos el vehiculo registrado, procedemos a cambiarle el tipo a residente.
                    _logger.LogDebug("Entered plate number is already registered. Updating the vehicle type...");
                    vehicle.VehicleTypeId = 2;

                    _ = await _vehicleRepository.UpdateAsync(vehicle);
                    _ = await _vehicleRepository.SaveAsync();

                    _logger.LogDebug("Vehicle type updated.");
                }

                //5. Mapeamos al dto de respuesta
                VehicleDto dto = _mapper.Map<VehicleDto>(vehicle);

                resp.Results.Add(dto);
                resp.Total = resp.Results.Count;
            }
            catch (Exception ex)
            {
                resp.Errors.Add(ex.ToString());
            }

            return resp;
        }
    }
}
