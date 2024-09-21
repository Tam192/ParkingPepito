using Application.Dtos;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings
{
    public class VehicleMapping : Profile
    {
        public VehicleMapping()
        {
            _ = CreateMap<Vehicle, VehicleTypeDto>().ReverseMap();
        }
    }
}
