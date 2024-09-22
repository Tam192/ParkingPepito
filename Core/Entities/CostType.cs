using Core.Interfaces.DbContext;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class CostType : IEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
