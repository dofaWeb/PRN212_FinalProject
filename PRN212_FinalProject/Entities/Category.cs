using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN212_FinalProject.Entities;

public partial class Category
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [NotMapped]
    public string? CategoryType { get; set; }
}
