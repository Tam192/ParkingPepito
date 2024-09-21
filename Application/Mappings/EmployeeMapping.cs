using Application.Dtos;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            _ = CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
