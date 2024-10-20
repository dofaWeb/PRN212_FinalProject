using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class OrderItem
{
    public string Id { get; set; } = null!;

    public string? OrderId { get; set; }

    public string? ProductItemId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ProductItem? ProductItem { get; set; }
}
