using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class AccountState
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
