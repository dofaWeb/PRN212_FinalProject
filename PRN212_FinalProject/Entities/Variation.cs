﻿using System;
using System.Collections.Generic;

namespace PRN212_FinalProject.Entities;

public partial class Variation
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<VariationOption> VariationOptions { get; set; } = new List<VariationOption>();
}
