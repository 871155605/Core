using System;
using System.Collections.Generic;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class SysRoleMenu
    {
        public int? SysRoleId { get; set; }
        public int? SysMenuId { get; set; }

        public virtual SysMenu SysMenu { get; set; }
        public virtual SysRole SysRole { get; set; }
    }
}
