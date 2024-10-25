using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class ProductConfiguration
{
    public string Id { get; set; } = null!;

    public string? ProductItemId { get; set; }

    public string? VariationOptionId { get; set; }

    public virtual ProductItem? ProductItem { get; set; }

    public virtual VariationOption? VariationOption { get; set; }
}
