using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Stay
{
    [Key]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int VehicleId { get; set; }

    public DateTime InitialDate { get; set; }

    public DateTime? FinalDate { get; set; }

    public int? DeleteEmployeeId { get; set; }

    public DateTime? DeleteDate { get; set; }

    public virtual Employee? DeleteEmployee { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
