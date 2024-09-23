using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class RegisterEntryUseCaseMapping : Profile
    {
        public RegisterEntryUseCaseMapping()
        {
            _ = CreateMap<RegisterEntryUseCaseDto, Vehicle>().ReverseMap();
            _ = CreateMap<RegisterEntryUseCaseDto, Stay>().ReverseMap();
            _ = CreateMap<Vehicle, Stay>()
                .ForMember(d => d.VehicleId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
