using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    public class JwtOptions : IConfigurableOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int Expiration { get; set; }
        public string DeniedAction { get; set; }
        public string LoginPath { get; set; }
    }
}
