using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class Order
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public DateTime Date { get; set; }

    public string? StateId { get; set; }

    public string? ProductItemId { get; set; }

    public int Price { get; set; }

    public virtual ProductItem? ProductItem { get; set; }

    public virtual OrderState? State { get; set; }

    public virtual Account? User { get; set; }
}
