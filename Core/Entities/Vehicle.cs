using Core.Interfaces.DbContext;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.Entities;

public partial class Vehicle : IEntity
{
    [Key]
    public int Id { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int VehicleTypeId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Stay> Stay { get; set; } = [];

    [JsonIgnore]
    public virtual VehicleType VehicleType { get; set; } = null!;
}
