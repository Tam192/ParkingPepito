using Core.Interfaces.DbContext;

namespace Core.Views;

public partial class ResidentDebts : IView
{
    public int VehicleId { get; set; }

    public string? PlateNumber { get; set; }

    public int? StayedMinutes { get; set; }

    public decimal? Cost { get; set; }

    public decimal? TotalCost { get; set; }
}
