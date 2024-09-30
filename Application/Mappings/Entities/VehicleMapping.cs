using Application.Dtos.Entities;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.Entities
{
    public class VehicleMapping : Profile
    {
        public VehicleMapping()
        {
            _ = CreateMap<Vehicle, VehicleDto>().ReverseMap();
        }
    }
}
