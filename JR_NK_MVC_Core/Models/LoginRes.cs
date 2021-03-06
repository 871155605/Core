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
        public AdminUser User { get; set; }
        public List<PermissionMenu> PermissionMenuList { get; set; }
        public object TokenJson { get; set; }
    }
}
