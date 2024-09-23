namespace Application.Dtos.UseCases
{
    public class RegisterExitUseCaseDto
    {
        public int EmployeeId { get; set; }
        public required string PlateNumber { get; set; }
        public DateTime FinalDate { get; set; }
    }
}
