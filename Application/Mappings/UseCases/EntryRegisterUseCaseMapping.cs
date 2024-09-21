using Application.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class EntryRegisterUseCaseMapping : Profile
    {
        public EntryRegisterUseCaseMapping()
        {
            _ = CreateMap<EntryRegisterUseCase, Vehicle>().ReverseMap();
            _ = CreateMap<EntryRegisterUseCase, Stay>().ReverseMap();
            _ = CreateMap<Vehicle, Stay>().ReverseMap();
        }
    }
}
