﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public partial class Employee
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public decimal? PhoneNumber { get; set; }

    public virtual ICollection<Stay> StayDeleteEmployee { get; set; } = [];

    public virtual ICollection<Stay> StayEmployee { get; set; } = [];
}