using System;
using System.Collections.Generic;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class AdminRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
    }
}
