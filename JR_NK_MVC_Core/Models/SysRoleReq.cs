using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class SysRoleReq : PageQueryReq
    {
        public string Name { get; set; }

        public int UserId { get; set; }
    }
}
