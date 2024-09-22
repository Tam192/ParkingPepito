using Core.Entities;
using Core.Interfaces.DbContext;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public partial class VehicleType : IEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public int CostTypeId { get; set; }

    [JsonIgnore]
    public virtual VehicleType CostType { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<VehicleType> InverseCostType { get; set; } = new List<VehicleType>();

    [JsonIgnore]
    public virtual ICollection<Vehicle> Vehicle { get; set; } = new List<Vehicle>();
}
