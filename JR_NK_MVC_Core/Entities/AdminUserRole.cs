using System;
using System.Collections.Generic;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class AdminUserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
