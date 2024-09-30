using Application.Dtos.Views;
using AutoMapper;
using Core.Views;

namespace Application.Mappings.UseCases
{
    public class ResidentDebtsReportUseCaseMapping : Profile
    {
        public ResidentDebtsReportUseCaseMapping()
        {
            CreateMap<ResidentDebts, ResidentDebtsDto>().ReverseMap();
        }
    }
}
