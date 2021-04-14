using JR_NK_MVC_Core.Common.Cache;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.JWT
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationSchemeProvider _schemes;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILoggerHelper _logger;
        private readonly ICache _cache;

        public PermissionHandler(IHttpContextAccessor accessor, IServiceProvider serviceProvider, ILoggerHelper logger,ICache cache)
        {
            _accessor = accessor;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _cache = cache;
            _schemes = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IAuthenticationSchemeProvider>();
        }

        // 重写异步处理程序
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //获取HttpContext
            var httpContext = _accessor.HttpContext;
            string questUrl = httpContext.Request.Path.Value.ToLower();
            _logger.Info(typeof(PermissionHandler), questUrl);
            //获取所有权限菜单存入缓存
            //List<PermissionItem> alls = _cache.Get<List<PermissionItem>>("AllPERMISSIONS");
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await _schemes.GetRequestHandlerSchemesAsync())
            {
                if (await handlers.GetHandlerAsync(httpContext, scheme.Name) is IAuthenticationRequestHandler handler && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            var defaultAuthenticate = await _schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)//拥有凭据
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                if (result?.Principal != null)//登录成功
                {
                    httpContext.User = result.Principal;
                    #region 自定义权限校验规则 获取当前用户的角色(此处业务用的是权限)信息
                    var currentUserRoles = (from item in httpContext.User.Claims
                                            where item.Type == PermissionRequirement.ClaimType
                                            select item.Value).ToList();
                    questUrl = questUrl.Replace("/", string.Empty);
                    if (currentUserRoles.Count <= 0 || !currentUserRoles.Where(w => questUrl.ToLower() == (w.ToLower())).Any())
                    {
                        context.Fail();
                        return;
                    }
                    #endregion
                    #region 校验TOKEN是否过期
                    if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    #endregion
                }
            }
            //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            if (!questUrl.Equals(PermissionRequirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
