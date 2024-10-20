using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class Supplier
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
