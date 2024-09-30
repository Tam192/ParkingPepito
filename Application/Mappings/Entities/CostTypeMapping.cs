using Application.Dtos.Entities;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings.Entities
{
    public class CostTypeMapping : Profile
    {
        public CostTypeMapping()
        {
            _ = CreateMap<CostType, CostTypeDto>().ReverseMap();
        }
    }
}
