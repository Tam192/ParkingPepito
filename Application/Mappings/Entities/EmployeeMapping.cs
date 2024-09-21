using Application.Dtos.Entities;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.Entities
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            _ = CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
