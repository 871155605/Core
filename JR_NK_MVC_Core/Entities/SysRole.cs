using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class SysRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Sort { get; set; }
        public string Remark { get; set; }
        public short? Status { get; set; }
        public short? IsDeleted { get; set; }
    }
}
