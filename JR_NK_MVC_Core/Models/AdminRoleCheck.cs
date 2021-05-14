using JR_NK_MVC_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class AdminRoleCheck : AdminRole
    {
        public bool IsChecked { get; set; }
    }
}
