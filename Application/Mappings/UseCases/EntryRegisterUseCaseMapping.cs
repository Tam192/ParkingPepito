using Application.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class EntryRegisterUseCaseMapping : Profile
    {
        public EntryRegisterUseCaseMapping()
        {
            _ = CreateMap<RegisterEntryUseCase, Vehicle>().ReverseMap();
            _ = CreateMap<RegisterEntryUseCase, Stay>().ReverseMap();
            _ = CreateMap<Vehicle, Stay>().ReverseMap();
        }
    }
}
