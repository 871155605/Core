using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Models;
using JR_NK_MVC_Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly ISysAdminService _adminService;
        private readonly ILoggerHelper _logger;
        public AdminController(ISysAdminService adminService, ILoggerHelper logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// 登录测试
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("login")]
        //[ApiExplorerSettings(IgnoreApi = true)]//屏蔽Swagger
        public async Task<GlobalResponse> Login([FromBody] LoginReq req)
        {
            try
            {
                SysUser user = await _adminService.LoadUserAsync(req.Username, req.Password);
                if (user == null) return GlobalResponse.Of(-1000, "账号密码错误");
                Dictionary<string, Object> keyValues = await _adminService.LoadUserPermissionMenusAsync(req.Username);
                keyValues.TryGetValue("permissions",out object permissionStringList);
                keyValues.TryGetValue("menus",out object permissionMenuList);
                if (permissionStringList == null) return GlobalResponse.Of(-1, "权限加载失败");
                if (permissionMenuList == null) return GlobalResponse.Of(-1, "权限菜单加载失败");
                var tokenJson = await _adminService.GetJwtTokenAsync((List<string>)permissionStringList, req.Username);
                return GlobalResponse.Of(new LoginRes { User = user, PermissionMenuList = (List<PermissionMenu>)permissionMenuList, TokenJson = tokenJson });
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.Message);
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 测试查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("user")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> Test([FromBody] SysUserReq req)
        {
            var resp = await _adminService.LoadUsersAsync(req);
            return GlobalResponse.Of(resp);
        }

        /// <summary>
        /// 测试查询1
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("user1")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> Test1([FromBody] SysUserReq req)
        {
            var resp = await _adminService.LoadUsersAsync(req);
            return GlobalResponse.Of(resp);
        }
    }
}
