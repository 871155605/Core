using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    public class JWTConfig
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }
        /// <summary>
        /// 登录路径
        /// </summary>
        public string LoginPath { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expiration { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SecretKey { get; set; }
    }
}
