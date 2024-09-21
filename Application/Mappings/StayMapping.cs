using Application.Dtos;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings
{
    public class StayMapping : Profile
    {
        public StayMapping()
        {
            _ = CreateMap<Stay, StayDto>().ReverseMap();
        }
    }
}
