using JR_NK_MVC_Core.Common.Cache;
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
        private readonly ICache _cache;
        public AdminController(ISysAdminService adminService, ILoggerHelper logger, ICache cache)
        {
            _adminService = adminService;
            _logger = logger;
            _cache = cache;
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
                AdminUser user = await _adminService.LoadUserAsync(req.Username, req.Password);
                if (user == null) return GlobalResponse.Of(-1000, "账号密码错误");
                Dictionary<string, Object> keyValues = _cache.Get<Dictionary<string, Object>>($"{ req.Username}-permission-menu");
                if (keyValues == null) keyValues = await _adminService.LoadUserPermissionMenusAsync(req.Username);
                keyValues.TryGetValue("permissions",out object permissionStringList);
                if (permissionStringList == null) return GlobalResponse.Of(-1, "权限加载失败");
                keyValues.TryGetValue("menus", out object permissionMenuList);
                if (permissionMenuList == null) return GlobalResponse.Of(-1, "权限菜单加载失败");
                //_cache.Set($"{req.Username}-permission-menu", keyValues); //方便测试可先取消缓存
                var tokenJson = await _adminService.GetJwtTokenAsync((List<string>)permissionStringList, req.Username);
                if (tokenJson == null) return GlobalResponse.Of(-1, "获取TOKEN失败");
                return GlobalResponse.Of(new LoginRes { User = user, PermissionMenuList = (List<PermissionMenu>)permissionMenuList, TokenJson = tokenJson });
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("addUser")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> AddUser([FromBody] AdminUser user) {
            try
            {
                bool flag = await _adminService.AddUserAsync(user);
                return GlobalResponse.Of(flag);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("updateUser")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> UpdateUser([FromBody] AdminUser user)
        {
            try
            {
                bool flag = await _adminService.UpdateUserAsync(user);
                return GlobalResponse.Of(flag);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("deleteUser")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> DeleteUser([FromBody] AdminUser user)
        {
            try
            {
                bool flag = await _adminService.DeleteUserAsync(user);
                return GlobalResponse.Of(flag);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("user")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> SelectUser([FromBody] SysUserReq req)
        {
            try
            {
                var resp = await _adminService.LoadUsersAsync(req);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("role")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> SelectRole([FromBody] SysRoleReq req)
        {
            try
            {
                var resp = await _adminService.LoadRolesAsync(req);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 加载权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet("loadPermissionTree")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> LoadPermissionTree(int roleId) {
            try
            {
                var resp = await _adminService.LoadPermissionTreeAsync(roleId);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController),e);
                return GlobalResponse.Of(-1,e.Message);
            }
        }

        /// <summary>
        /// 加载选择框角色列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("loadRoleCheckBox")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> LoadRoleCheckBox(int userId) {
            try
            {
                var resp = await _adminService.LoadRoleCheckBoxAsync(userId);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 保存用户的角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("saveUserRole")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> SaveUserRole([FromBody] SaveUserRoleMenuReq req)
        {
            try
            {
                var resp = await _adminService.SaveUserRoleAsync(req.CheckedRoleList,req.UserId);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 保存角色的权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("saveRoleMenu")]
        [Authorize("Custom")]
        public async Task<GlobalResponse> SaveRoleMenu([FromBody] SaveUserRoleMenuReq req)
        {
            try
            {
                var resp = await _adminService.SaveRoleMenuAsync(req.CheckedMenuList, req.RoleId);
                return GlobalResponse.Of(resp);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(AdminController), e.ToString());
                return GlobalResponse.Of(-1, e.Message);
            }
        }
    }
}
