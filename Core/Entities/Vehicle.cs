using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Vehicle
{
    [Key]
    public int Id { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int VehicleTypeId { get; set; }

    public virtual ICollection<Stay> Stay { get; set; } = [];

    public virtual VehicleType VehicleType { get; set; } = null!;
}
