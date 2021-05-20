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
        public Task<AdminUser> LoadUserAsync(string account, string password);

        /// <summary>
        /// 获取用户TOKEN
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="uniqueName">账号</param>
        /// <returns></returns>
        public Task<string> GetJwtTokenAsync(List<string> permissions,string uniqueName);

        /// <summary>
        /// 加载用户权限
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<Dictionary<string,Object>> LoadUserPermissionMenusAsync(string account);

        /// <summary>
        /// 加载权限树
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<Dictionary<string, Object>> LoadPermissionTreeAsync(int roleId);

        /// <summary>
        /// 加载授权选择盒子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<Dictionary<string, Object>> LoadRoleCheckBoxAsync(int userId);

        /// <summary>
        /// 加载所有用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageQueryRes> LoadUsersAsync(SysUserReq req);

        /// <summary>
        /// 加载所有角色
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageQueryRes> LoadRolesAsync(SysRoleReq req);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<bool> AddRoleAsync(AdminRole role);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<bool> UpdateRoleAsync(AdminRole role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<bool> DeleteRoleAsync(AdminRole role);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> AddUserAsync(AdminUser user);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UpdateUserAsync(AdminUser user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> DeleteUserAsync(AdminUser user);

        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="checkedRoleList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<bool> SaveUserRoleAsync(List<int> checkedRoleList,int userId);

        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="checkedMenuList"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<bool> SaveRoleMenuAsync(List<int> checkedMenuList, int roleId);
    }
}
