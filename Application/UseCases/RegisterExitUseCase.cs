using Application.Dtos;
using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repository;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public interface IRegisterExitUseCase
    {
        public Task<BaseResponse<decimal>> RegisterExitAsync(RegisterExitUseCaseDto request);
    }

    public class RegisterExitUseCase(ILogger<RegisterEntryUseCase> logger, IEntitiesRepository<Vehicle> vehicleRepository, IEntitiesRepository<Stay> stayRepository, IMapper mapper) : IRegisterExitUseCase
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEntitiesRepository<Vehicle> _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IEntitiesRepository<Stay> _stayRepository = stayRepository ?? throw new ArgumentNullException(nameof(stayRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<BaseResponse<decimal>> RegisterExitAsync(RegisterExitUseCaseDto request)
        {
            BaseResponse<decimal> resp = new();

            try
            {
                //1. Buscamos en la base de datos el vehiculo
                _logger.LogDebug("Checking if the plate number enterer is registered.");
                Vehicle? vehicle = await _vehicleRepository.GetAsync(vehicle => vehicle.PlateNumber == request.PlateNumber, vehicle => vehicle.OrderBy(x => x.Id), "VehicleType");

                if (vehicle is null)
                {
                    _logger.LogDebug("Error retrieving the vehicle with plate number {0}", request.PlateNumber);
                    resp = new BaseResponse<decimal>
                    {
                        Errors = ["The plate number {0} is not registered on the system.", request.PlateNumber],
                        Results = [],
                        Total = 0
                    };

                    return resp;
                }

                //2. Si existe el vehiculo y no es de tipo residente, buscamos su estancia abierta
                if (vehicle.VehicleTypeId != 2)
                {
                    Stay? stay = await GetVehicleOpenedStay(vehicle);

                    if (stay is null)
                    {
                        _logger.LogDebug("Error retrieving the opened stay for the vehicle with plate number {0}", request.PlateNumber);
                        resp = new BaseResponse<decimal>
                        {
                            Errors = ["Error retrieving the opened stay for the vehicle with plate number {0}", request.PlateNumber],
                            Results = [],
                            Total = 0
                        };

                        return resp;
                    }

                    //3. Finalizamos la estancia abierta del vehiculo
                    stay = await TerminateStay(stay, request);

                    if (stay.FinalDate is null)
                    {
                        _logger.LogDebug("Error terminating the opened stay for the vehicle with plate number {0}", request.PlateNumber);
                        resp = new BaseResponse<decimal>
                        {
                            Errors = ["Error terminating the opened stay for the vehicle with plate number {0}", request.PlateNumber],
                            Results = [],
                            Total = 0
                        };

                        return resp;
                    }

                    //4. Si es de tipo no residente, obtenemos el importe total a pagar
                    if (vehicle.VehicleTypeId == 3)
                    {
                        decimal debt = GetVehicleTotalDebt(stay, vehicle, request);

                        resp.Results.Add(debt);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Errors.Add(ex.ToString());
            }

            resp.Total = 1;
            return resp;
        }

        private async Task<Stay?> GetVehicleOpenedStay(Vehicle vehicle)
        {
            _logger.LogDebug("Retrieving the opened stay for the plate number {0}", vehicle.PlateNumber);

            Stay? stay = await _stayRepository.GetAsync(stay => stay.VehicleId == vehicle.Id && stay.DeleteDate == null, stay => stay.OrderBy(x => x.Id), null);

            return stay;
        }

        private async Task<Stay> TerminateStay(Stay stay, RegisterExitUseCaseDto request)
        {
            _logger.LogDebug("Terminating the stay with id {0}", stay.Id);

            Stay terminedStay = stay;
            terminedStay = _mapper.Map(request, terminedStay);

            _ = await _stayRepository.UpdateAsync(terminedStay);
            _ = await _stayRepository.SaveAsync();

            _logger.LogDebug("Stay terminates.");

            return terminedStay;
        }

        private static decimal GetVehicleTotalDebt(Stay stay, Vehicle vehicle, RegisterExitUseCaseDto request)
        {
            decimal minutes = (decimal)(request.FinalDate - stay.InitialDate).TotalMinutes;
            decimal cost = vehicle.VehicleType.Cost;

            return cost * minutes;
        }
    }
}
