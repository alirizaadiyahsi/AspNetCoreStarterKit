﻿using System;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreStarterKit.WebApi.Core.Configuration
{
    public class JwtTokenConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
