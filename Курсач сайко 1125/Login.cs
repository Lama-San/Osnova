using System;
using System.Collections.Generic;

namespace Курсач_сайко_1125;

public partial class Login
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string PassportNumber { get; set; } = null!;
}
