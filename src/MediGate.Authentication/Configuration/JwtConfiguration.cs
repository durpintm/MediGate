using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediGate.Authentication.Configuration
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}