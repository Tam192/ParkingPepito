using Application.Dtos.UseCases;

namespace FunctionApp.Tests.Utils.MockedObjects.Function
{
    internal static class RegisterExitFunctionMockedObject
    {
        public static RegisterExitUseCaseDto GenerateRegisterExitUseCaseDtoMockedObject()
        {
            RegisterExitUseCaseDto dto = new()
            { 
                PlateNumber = "PlateNumber",
                EmployeeId = 1,
                FinalDate = DateTime.MaxValue
            };

            return dto;
        }
    }
}
