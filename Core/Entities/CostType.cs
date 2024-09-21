using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class CostType
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
