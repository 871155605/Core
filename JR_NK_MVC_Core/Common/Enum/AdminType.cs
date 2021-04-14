using System.ComponentModel;

namespace JR_NK_MVC_Core.Common.Enum
{
    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AdminType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SuperAdmin = 1,

        /// <summary>
        /// 非管理员
        /// </summary>
        [Description("非管理员")]
        None = 2
    }
}
