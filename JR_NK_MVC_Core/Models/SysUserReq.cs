using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class SysUserReq : PageQueryReq
    {
        public String Name { get; set; }
        public String NickName { get; set; }
    }
}
