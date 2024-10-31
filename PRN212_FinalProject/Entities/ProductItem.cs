using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN212_FinalProject.Entities;

public partial class ProductItem
{
    public string Id { get; set; } = null!;

    public string? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ImportPrice { get; set; }

    public int? SellingPrice { get; set; }

    public decimal? Discount { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Product? Product { get; set; }

    public virtual ICollection<ProductConfiguration> ProductConfigurations { get; set; } = new List<ProductConfiguration>();

    [NotMapped]
    public string? Ram { get; set; }

    [NotMapped]
    public string? Option { get; set; }

    [NotMapped]
    public string? Storage { get; set; }

    [NotMapped]
    public int? PriceAfterDiscount { get; set; }

    [NotMapped]
    public int? Profit { get; set; }
}
