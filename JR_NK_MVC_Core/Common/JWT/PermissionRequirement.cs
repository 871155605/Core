using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public static string DeniedAction { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public static string ClaimType { internal get; set; } = ClaimTypes.Role;
        /// <summary>
        /// 登录路径
        /// </summary>
        public static string LoginPath { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public static string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public static string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public static TimeSpan Expiration { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public static SigningCredentials SigningCredentials { get; set; }
    }
}
