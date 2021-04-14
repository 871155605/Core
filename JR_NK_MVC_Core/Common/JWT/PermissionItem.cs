using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    /// <summary>
    /// 用户或角色或其他凭据实体
    /// </summary>
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Link { get; set; }
        /// <summary>
        /// 权限或按钮其他凭据名称
        /// </summary>
        public virtual string Permission { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public int? Pid { get; set; }
    }
}
