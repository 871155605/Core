using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class SysUser
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public DateTime? Birthday { get; set; }
        public short? Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public short? Status { get; set; }
        public short? IsDeleted { get; set; }
    }
}
