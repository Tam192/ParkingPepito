using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.UseCases
{
    public class StartMonthUseCaseMapping : Profile
    {
        public StartMonthUseCaseMapping()
        {
            _ = CreateMap<StartMonthUseCaseDto, Stay>()
                .ForMember(d => d.EmployeeId, opt => opt.Ignore())
                .ForMember(d => d.DeleteEmployeeId, opt => opt.MapFrom(s => s.EmployeeId))
                .ForMember(d => d.DeleteDate, opt => opt.MapFrom(s => DateTime.Now))
                .ReverseMap();
        }
    }
}
