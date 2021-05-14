using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class PermissionTree
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public List<PermissionTree> Children { get; set; }
    }
}
