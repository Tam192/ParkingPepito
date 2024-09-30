using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class RegisterOfficialVehicleUseCaseMapping : Profile
    {
        public RegisterOfficialVehicleUseCaseMapping()
        {
            _ = CreateMap<RegisterOfficialVehicleDto, Vehicle>()
                .ForMember(d => d.VehicleTypeId, opt => opt.MapFrom(s => 1))
                .ReverseMap();
        }
    }
}
