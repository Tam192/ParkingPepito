using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class VehicleTypeMapping : Profile
    {
        public VehicleTypeMapping()
        {
            _ = CreateMap<VehicleType, VehicleTypeDto>().ReverseMap();
        }
    }
}
