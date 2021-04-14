using System;
using System.Collections.Generic;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class SysUserRole
    {
        public int SysUserId { get; set; }
        public int SysRoleId { get; set; }

        public virtual SysRole SysRole { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
