using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Picture { get; set; }

    public string? Description { get; set; }

    public string? CategoryId { get; set; }
  

    public string? SupplierId { get; set; }
   

    public virtual Category? Category { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

    public virtual Supplier? Supplier { get; set; }
}
