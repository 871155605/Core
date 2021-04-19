using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Service
{
    /// <summary>
    /// 权限管理服务
    /// </summary>
    public interface ISysAdminService
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<SysUser> LoadUserAsync(string account, string password);

        /// <summary>
        /// 获取用户TOKEN
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="uniqueName">账号</param>
        /// <returns></returns>
        public Task<string> GetJwtTokenAsync(List<PermissionItem> permissions,string uniqueName);

        /// <summary>
        /// 加载所有权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<List<PermissionItem>> LoadPermissionItemsAsync(SysUser user);

        /// <summary>
        /// 加载所有用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageQueryRes> LoadUsersAsync(SysUserReq req);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> AddUserAsync(SysUser user);

        /// <summary>
        /// 给用户添加角色
        /// </summary>
        /// <param name="urs"></param>
        /// <returns></returns>
        public Task<bool> AddUserRolesAsync(List<SysUserRole> urs);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<bool> AddRoleAsync(SysRole role);

        /// <summary>
        /// 给角色添加权限
        /// </summary>
        /// <param name="rms"></param>
        /// <returns></returns>
        public Task<bool> AddRoleMenusAsync(List<SysRoleMenu> rms);
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public Task<bool> AddMenuAsync(SysMenu menu);
    }
}
