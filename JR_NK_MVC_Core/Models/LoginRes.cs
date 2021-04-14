using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class LoginRes
    {
        public SysUser User { get; set; }
        public List<PermissionItem> PermissionItems { get; set; }
        public object TokenJson { get; set; }
    }
}
