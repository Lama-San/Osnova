﻿using System;
using System.Collections.Generic;

namespace Курсач_сайко_1125;

public partial class Zap
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Gpa { get; set; }

    public string Spec { get; set; } = null!;

    public virtual Yeszap? Yeszap { get; set; }
}
