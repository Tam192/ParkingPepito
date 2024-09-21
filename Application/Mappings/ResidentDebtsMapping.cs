using Application.Dtos;
using AutoMapper;
using Core.Views;

namespace Application.Mappings
{
    public class ResidentDebtsMapping : Profile
    {
        public ResidentDebtsMapping()
        {
            _ = CreateMap<ResidentDebts, ResidentDebtsDto>().ReverseMap();
        }
    }
}
