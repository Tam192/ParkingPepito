using Application.Dtos.Entities;
using Application.Dtos.UseCases;

namespace FunctionApp.Tests.Utils.MockedObjects.Function
{
    internal static class RegisterEntryFunctionMockedObjects
    {
        public static RegisterEntryUseCaseDto GenerateRegisterEntryUseCaseDtoMockedObject()
        {
            RegisterEntryUseCaseDto dto = new()
            {
                EmployeeId = 1,
                PlateNumber = "PlateNumber",
                InitialDate = DateTime.MinValue
            };

            return dto;
        }

        public static StayDto GenerateStayDtoMockedObject()
        {
            StayDto dto = new()
            {
                DeleteDate = DateTime.MaxValue,
                EmployeeId = 1,
                FinalDate = DateTime.MaxValue,
                DeleteEmployeeId = 1,
                Id = 1,
                InitialDate = DateTime.MinValue,
                VehicleId = 1
            };

            return dto;
        }
    }
}
