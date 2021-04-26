using JR_NK_MVC_Core.Common.until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Models
{
    public class TestModel
    {
        [Excel("存货分类编码")]
        public string InventoryCode { get; set; }

        [Excel("存货分类名称")]
        public string InventoryName { get; set; }

        [Excel("规格型号")]
        public string SPU{ get; set; }

        [Excel("计量单位")]
        public string Unit { get; set; }

        [Excel("数量")]
        public decimal Amount { get; set; }

        [Excel("金额")]
        public decimal Money { get; set; }

        [Excel("单价")]
        public decimal UnitPrice { get; set; }
        
        [Excel("毛重")]
        public string RoughWeight { get; set; }

        [Excel("净重")]
        public string NetWeight { get; set; }
    }
}