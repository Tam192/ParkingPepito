using Core.Interfaces.DbContext;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities;

public partial class Stay : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int VehicleId { get; set; }

    public DateTime InitialDate { get; set; }

    public DateTime? FinalDate { get; set; }

    public int? DeleteEmployeeId { get; set; }

    public DateTime? DeleteDate { get; set; }

    [JsonIgnore]
    public virtual Employee? DeleteEmployee { get; set; }

    [JsonIgnore]
    public virtual Employee Employee { get; set; } = null!;

    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
