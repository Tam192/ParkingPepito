using Application.Dtos.Entities;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Entities
{
    public class VehicleTypeMapping : Profile
    {
        public VehicleTypeMapping()
        {
            _ = CreateMap<VehicleType, VehicleTypeDto>().ReverseMap();
        }
    }
}
