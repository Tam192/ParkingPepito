using Application.Dtos.Views;
using AutoMapper;
using Core.Views;

namespace Application.Mappings.Views
{
    public class ResidentDebtsMapping : Profile
    {
        public ResidentDebtsMapping()
        {
            _ = CreateMap<ResidentDebts, ResidentDebtsDto>().ReverseMap();
        }
    }
}
