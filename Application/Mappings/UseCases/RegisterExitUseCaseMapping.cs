using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class RegisterExitUseCaseMapping : Profile
    {
        public RegisterExitUseCaseMapping()
        {
            _ = CreateMap<RegisterExitUseCaseDto, Stay>()
                .ForMember(d => d.EmployeeId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
