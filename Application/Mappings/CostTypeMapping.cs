using Application.Dtos;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings
{
    public class CostTypeMapping : Profile
    {
        public CostTypeMapping()
        {
            _ = CreateMap<CostType, CostTypeDto>().ReverseMap();
        }
    }
}
