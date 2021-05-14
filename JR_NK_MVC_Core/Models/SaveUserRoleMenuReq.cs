using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class SaveUserRoleMenuReq
    {
        public List<int> CheckedRoleList { get; set; }
        public List<int> CheckedMenuList { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}
