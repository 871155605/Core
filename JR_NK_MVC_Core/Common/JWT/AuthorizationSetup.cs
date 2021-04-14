using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, PermissionRequirement permissionRequirement)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = PermissionRequirement.SigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = PermissionRequirement.Issuer,
                ValidateAudience = true,
                ValidAudience = PermissionRequirement.Audience,
                ValidateLifetime = true,
                ClockSkew = PermissionRequirement.Expiration,
                RequireExpirationTime = true,
            };
            // 添加JwtBearer服务
            // ① 核心之一，配置授权服务，也就是具体的规则，已经对应的权限策略，比如公司不同权限的门禁卡
            services.AddAuthorization(options =>
            {
                // 自定义基于策略的授权权限
                options.AddPolicy("Custom",
                         policy => policy.Requirements.Add(permissionRequirement));
            })
            // ② 核心之二，必需要配置认证服务，这里是jwtBearer默认认证，比如光有卡没用，得能识别他们
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // ③ 核心之三，针对JWT的配置，比如门禁是如何识别的，是放射卡，还是磁卡
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
            });
            // 依赖注入，将自定义的授权处理器 匹配给官方授权处理器接口，这样当系统处理授权的时候，就会直接访问我们自定义的授权处理器了。
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            // 将授权必要类注入生命周期内
            //services.AddSingleton(permissionRequirement);
        }
    }
}
