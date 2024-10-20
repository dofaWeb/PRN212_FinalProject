using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class VariationOption
{
    public string Id { get; set; } = null!;

    public string? VariationId { get; set; }

    public string Value { get; set; } = null!;

    public virtual Variation? Variation { get; set; }

    public virtual ICollection<ProductItem> ProductItems { get; set; } = new List<ProductItem>();
}
