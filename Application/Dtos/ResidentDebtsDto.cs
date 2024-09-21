namespace Application.Dtos
{
    public class ResidentDebtsDto
    {
        public int? VehicleId { get; set; }
        public string? PlateNumber { get; set; }
        public int? StayedMinutes { get; set; }
        public decimal? Cost { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
