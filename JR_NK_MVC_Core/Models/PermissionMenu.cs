using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class PermissionMenu
    {
        public string IconCls { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<PermissionMenu> Children { get; set; }
        public List<string> Button { get; set;}
    }
}
