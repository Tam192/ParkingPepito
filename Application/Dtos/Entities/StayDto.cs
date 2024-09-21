namespace Application.Dtos.Entities
{
    public class StayDto
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? VehicleId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public int? DeleteEmployeeId { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
