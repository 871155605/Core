using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class LoginReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
    }
}
