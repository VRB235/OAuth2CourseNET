using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class Constants
    {
        public const string Audiance = "https://localhost:44390/"; // A quien va a estar dirigido el token
        public const string Issuer = Audiance; // Quien da el token
        public const string Secret = "not_to_short_secret_otherwise_it_might_throw_an_error"; // Quien da el token
    }
}
