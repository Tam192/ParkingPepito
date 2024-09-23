using Application.Dtos.UseCases;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Mappings.UseCases
{
    public class RegisterExitUseCaseMapping : Profile
    {
        public RegisterExitUseCaseMapping() 
        {
            CreateMap<RegisterExitUseCaseDto, Stay>()
                .ForMember(d => d.EmployeeId, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
