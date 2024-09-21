namespace Application.Dtos.UseCases
{
    public class RegisterEntryUseCaseDto
    {
        public int EmployeeId { get; set; }
        public required string PlateNumber { get; set; }
        public DateTime InitialDate { get; set; }
    }
}
