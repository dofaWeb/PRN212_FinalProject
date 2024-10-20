using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class ProductItem
{
    public string Id { get; set; } = null!;

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ImportPrice { get; set; }

    public int? SellingPrice { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<VariationOption> VariationOptions { get; set; } = new List<VariationOption>();
}
