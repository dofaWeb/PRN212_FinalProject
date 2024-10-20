using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? RoleId { get; set; }

    public string? StateId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual RoleName? Role { get; set; }

    public virtual AccountState? State { get; set; }
}
