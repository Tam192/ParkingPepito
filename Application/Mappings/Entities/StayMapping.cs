using Application.Dtos.Entities;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.Entities
{
    public class StayMapping : Profile
    {
        public StayMapping()
        {
            _ = CreateMap<Stay, StayDto>().ReverseMap();
            //_ = CreateMap<Stay, RegisterEntryUseCaseDto>().ReverseMap();
        }
    }
}
