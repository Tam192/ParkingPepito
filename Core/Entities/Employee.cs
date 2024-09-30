using Core.Interfaces.DbContext;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entities;

public partial class Employee : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? PhoneNumber { get; set; }

    [JsonIgnore]
    public virtual ICollection<Stay> StayDeleteEmployee { get; set; } = [];

    [JsonIgnore]
    public virtual ICollection<Stay> StayEmployee { get; set; } = [];
}
