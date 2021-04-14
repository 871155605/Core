using System.ComponentModel;

namespace JR_NK_MVC_Core.Common.Enum
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        MALE = 1,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        FEMALE = 2,

        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        UNKNOWN = 3
    }
}
