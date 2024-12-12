using System;

namespace FinanceApp.Application.Services;

public class JwtSettings
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string SecretKey { get; set; }
}

