using Core.Interfaces.DbContext;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities;

public partial class Vehicle : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string PlateNumber { get; set; } = null!;

    public int VehicleTypeId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Stay> Stay { get; set; } = [];

    [JsonIgnore]
    public virtual VehicleType VehicleType { get; set; } = null!;
}
