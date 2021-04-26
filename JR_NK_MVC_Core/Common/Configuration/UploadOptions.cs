using Furion.ConfigurableOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.Configuration
{
    /// <summary>
    /// 上传文件配置类
    /// </summary>
    public class UploadOptions : IConfigurableOptions
    {
        /// <summary>
        /// 存储类型
        /// </summary>
        public SaveType SaveType { get; set; }
        /// <summary>
        /// 存储地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 导入临时存储文件地址
        /// </summary>
        public string ImportPath { get; set; }
    }

    public enum SaveType
    {
        /// <summary>
        /// 终端缓存
        /// </summary>
        TerminalMemoryCache,

        /// <summary>
        /// 本地缓存
        /// </summary>
        LoacalCache
    }
}
