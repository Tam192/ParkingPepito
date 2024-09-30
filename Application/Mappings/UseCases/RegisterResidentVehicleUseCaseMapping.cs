using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class RegisterResidentVehicleUseCaseMapping : Profile
    {
        public RegisterResidentVehicleUseCaseMapping()
        {
            _ = CreateMap<RegisterResidentVehicleDto, Vehicle>()
                .ForMember(d => d.VehicleTypeId, opt => opt.MapFrom(s => 2))
                .ReverseMap();
        }
    }
}
