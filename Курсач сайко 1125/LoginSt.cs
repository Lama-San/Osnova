using System;
using System.Collections.Generic;

namespace Курсач_сайко_1125;

public partial class Loginst
{
    public int Id { get; set; }

    public string StudentName { get; set; } = null!;

    public string StudentPassword { get; set; } = null!;

    public string? StudentEmail { get; set; }

    public string PassportNumber { get; set; } = null!;

    public string? StudentGpa { get; set; }

    public string? StudentSpec { get; set; }

    public string Status { get; set; } = null!;
}
