using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class PageQueryRes
    {
        public int TotalCount { get; set; }

        public IEnumerable<Object> TableData {get; set;}
    }
}
