using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public partial class VehicleType
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int CostTypeId { get; set; }

    public virtual VehicleType CostType { get; set; } = null!;

    public virtual ICollection<VehicleType> InverseCostType { get; set; } = new List<VehicleType>();

    public virtual ICollection<Vehicle> Vehicle { get; set; } = new List<Vehicle>();
}
