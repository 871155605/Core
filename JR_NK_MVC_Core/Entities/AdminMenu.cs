using System;
using System.Collections.Generic;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class AdminMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Pid { get; set; }
        public string Code { get; set; }
        public int? Type { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string Link { get; set; }
    }
}
