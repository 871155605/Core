using Furion.LinqBuilder;
using JR_NK_MVC_Core.Common.Cache;
using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Common.Until;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Service.Impl
{
    public class SysAdminServiceImpl : ISysAdminService
    {
        private readonly ILoggerHelper _logger;
        private readonly JRDBContext _dbcontext;
        private readonly ICache _cache;

        public SysAdminServiceImpl(ILoggerHelper logger,JRDBContext dbcontext,ICache cache) {
            _logger = logger;
            _dbcontext = dbcontext;
            _cache = cache;
        }

        public async Task<AdminUser> LoadUserAsync(string account, string password)
        {
            List<AdminUser> users = await _dbcontext.AdminUsers.Where(_ => _.Account == account && _.Password == password).ToListAsync();
            AdminUser user = null;
            if (users != null && users.Count > 0) user = users[0];
            return user;
        }

        public async Task<Dictionary<string,Object>> LoadUserPermissionMenusAsync(string account) {
            Dictionary<string, Object> keyValues = new();
            List<string> permissionStringList = new();
            List<PermissionMenu> permissionMenuList = new();
            keyValues.Add("menus",permissionMenuList);
            keyValues.Add("permissions", permissionStringList);
            string sql1 = $@"SELECT DISTINCT m.ID,m.Name,m.PID,m.Code,m.Type,m.Icon,m.Permission,m.Link
                            FROM admin_user u,admin_user_role ur,admin_role_menu rm,admin_menu m
                            WHERE u.Account = '{account}' AND u.ID = ur.UserId AND ur.RoleId = rm.RoleId AND rm.MenuId = m.ID AND m.Type = 1";
            //_logger.Info(typeof(SysAdminServiceImpl),sql1);
            List<AdminMenu> sysMenus1 = (List<AdminMenu>) await DapperSqlHelper.QueryAsync<AdminMenu>(sql1);
            foreach (var item1 in sysMenus1)
            {
                PermissionMenu permission1 = new() { IconCls = item1.Icon, Name = item1.Name,Path= item1.Link, Code = item1.Code };
                permissionStringList.Add(item1.Permission);
                permissionMenuList.Add(permission1);
                string sql2 = $@"SELECT m.ID,m.Name,m.PID,m.Code,m.Type,m.Icon,m.Permission,m.Link 
                              FROM admin_user u,admin_user_role ur,admin_role_menu rm,admin_menu m
                              WHERE u.Account = '{account}' AND u.ID = ur.UserId AND ur.RoleId = rm.RoleId AND rm.MenuId = m.ID AND m.Type = 2 AND m.PID = {item1.Id}";
                //_logger.Info(typeof(SysAdminServiceImpl), sql2);
                List<AdminMenu> sysMenus2 = (List<AdminMenu>)await DapperSqlHelper.QueryAsync<AdminMenu>(sql2);
                if (sysMenus2 != null && sysMenus2.Count > 0)
                {
                    List<PermissionMenu> Children1 = new();
                    permission1.Children = Children1;
                    foreach (var item2 in sysMenus2)
                    {
                        permissionStringList.Add(item2.Permission);
                        PermissionMenu permission2 = new() { IconCls = item2.Icon, Name = item2.Name, Path = item2.Link,Code=item2.Code };
                        Children1.Add(permission2);
                        string sql3 = $@"SELECT m.ID,m.Name,m.PID,m.Code,m.Type,m.Icon,m.Permission,m.Link 
                              FROM admin_user u,admin_user_role ur,admin_role_menu rm,admin_menu m
                              WHERE u.Account = '{account}' AND u.ID = ur.UserId AND ur.RoleId = rm.RoleId AND rm.MenuId = m.ID AND m.Type = 3 AND m.PID = {item2.Id}";
                        //_logger.Info(typeof(SysAdminServiceImpl), sql3);
                        List<AdminMenu> sysMenus3 = (List<AdminMenu>)await DapperSqlHelper.QueryAsync<AdminMenu>(sql3);
                        if (sysMenus3 != null && sysMenus3.Count > 0)
                        {
                            List<PermissionMenu> Children2 = new();
                            permission2.Children = Children2;
                            foreach (var item3 in sysMenus3)
                            {
                                permissionStringList.Add(item3.Permission);
                                PermissionMenu permission3 = new() { IconCls = item3.Icon, Name = item3.Name, Path = item3.Link, Code = item3.Code };
                                Children2.Add(permission3);
                                List<string> button3 = await LoadPermissionButtonAsync(account,item3, permissionStringList);
                                permission3.Button = button3;
                            }
                        }
                        else {
                            List<string> button2 = await LoadPermissionButtonAsync(account,item2,permissionStringList);
                            permission2.Button = button2;
                        }
                    }
                }
                else {
                    List<string> button1 = await LoadPermissionButtonAsync(account,item1,permissionStringList);
                    permission1.Button = button1;
                }
            }
            return keyValues;
        }

        public async Task<Dictionary<string, Object>> LoadPermissionTreeAsync(int roleId) {
            Dictionary<string, Object> keyValues = new();
            string sql1 = $@"SELECT DISTINCT admin_menu.ID,admin_menu.Name,admin_menu.PID FROM admin_role_menu,admin_menu
            WHERE admin_role_menu.MenuId = admin_menu.ID AND admin_menu.Type = 1;";
            //_logger.Info(typeof(SysAdminServiceImpl), "LoadPermissionTreeAsync-sql1:" + sql1);
            List<PermissionTree> permissionTree1 = (List<PermissionTree>)await DapperSqlHelper.QueryAsync<PermissionTree>(sql1);//加载所有权限树
            List<int> checked_keys = new();
            List<int> expanded_keys = new();
            keyValues.Add("checked_keys", checked_keys);
            keyValues.Add("expanded_keys", expanded_keys);
            keyValues.Add("permissionTree", permissionTree1);
            await LoadPermissionButtonAsync(roleId,checked_keys,expanded_keys);//加载角色拥有的权限
            foreach (var item1 in permissionTree1)
            {
                string sql2 = $"SELECT ID,Name,PID FROM admin_menu WHERE PID = {item1.Id} AND Type = 2;";
                //_logger.Info(typeof(SysAdminServiceImpl), "LoadPermissionTreeAsync-sql2:"+sql2);
                List<PermissionTree> permissionTree2 = (List<PermissionTree>)await DapperSqlHelper.QueryAsync<PermissionTree>(sql2);
                if (permissionTree2 != null && permissionTree2.Count > 0)
                {
                    item1.Children = permissionTree2;
                    foreach (var item2 in permissionTree2)
                    {
                        string sql3 = $"SELECT ID,Name,PID FROM admin_menu WHERE PID = {item2.Id} AND Type = 3;";
                        //_logger.Info(typeof(SysAdminServiceImpl), "LoadPermissionTreeAsync-sql3:" + sql3);
                        List<PermissionTree> permissionTree3 = (List<PermissionTree>)await DapperSqlHelper.QueryAsync<PermissionTree>(sql3);
                        if (permissionTree3 != null && permissionTree3.Count > 0)
                        {
                            item2.Children = permissionTree3;
                            foreach (var item3 in permissionTree3)
                            {
                                await LoadPermissionButtonAsync(item3);
                            }
                        }
                        else {
                            await LoadPermissionButtonAsync(item2);
                        }
                    }
                }
                else
                {
                    await LoadPermissionButtonAsync(item1);
                }
            }
            return keyValues;
        }

        /// <summary>
        /// 加载选择树
        /// 加载展开树
        /// 需要优化SQL
        /// </summary>
        private static async Task LoadPermissionButtonAsync(int roleId,List<int> checked_keys,List<int> expanded_keys)
        {
            string checked_sql = $@"SELECT admin_menu.ID,admin_menu.Name 
                            FROM admin_menu,admin_role_menu 
                            WHERE admin_menu.ID = admin_role_menu.MenuId AND admin_menu.type = 4 AND admin_role_menu.RoleId = {roleId}";
            //_logger.Info(typeof(SysAdminServiceImpl), "checked_sql:"+ checked_sql);
            List<PermissionTree> checkedPermissionTree = (List<PermissionTree>)await DapperSqlHelper.QueryAsync<PermissionTree>(checked_sql);
            if (checkedPermissionTree != null && checkedPermissionTree.Count > 0) {
                foreach (var item in checkedPermissionTree)
                {
                    checked_keys.Add(item.Id);//添加选择
                }
            }
            string expanded_sql = $@"SELECT admin_menu.ID,admin_menu.Name 
                            FROM admin_menu,admin_role_menu 
                            WHERE admin_menu.ID = admin_role_menu.MenuId AND admin_menu.type != 4 AND admin_role_menu.RoleId = {roleId}";
            //_logger.Info(typeof(SysAdminServiceImpl), "expanded_sql:"+ expanded_sql);
            List<PermissionTree> expandedPermissionTree = (List<PermissionTree>)await DapperSqlHelper.QueryAsync<PermissionTree>(expanded_sql);
            if (expandedPermissionTree != null && expandedPermissionTree.Count > 0)
            {
                foreach (var item in expandedPermissionTree)
                {
                    expanded_keys.Add(item.Id);//添加展开
                }
            }
        }

        /// <summary>
        /// 加载单个节点下的所有按钮
        /// </summary>
        /// <param name="permissionTree"></param>
        private static async Task LoadPermissionButtonAsync(PermissionTree permissionTree)
        {
            string sql = "SELECT ID,Name,PID FROM admin_menu WHERE pid = @id AND type = 4";
            List<PermissionTree> permissionTrees = (List<PermissionTree>) await DapperSqlHelper.QueryAsync<PermissionTree>(sql, permissionTree);
            permissionTree.Children = permissionTrees;
        }

        /// <summary>
        /// 加载角色权限按钮
        /// </summary>
        /// <param name="account"></param>
        /// <param name="AdminMenu"></param>
        /// <param name="permissionStringList"></param>
        /// <returns></returns>
        private static async Task<List<string>> LoadPermissionButtonAsync(string account, AdminMenu AdminMenu,List<string> permissionStringList)
        {
            string sql = $@"SELECT m.ID,m.Name,m.PID,m.Code,m.Type,m.Icon,m.Permission,m.Link 
                            FROM admin_user u,admin_user_role ur,admin_role_menu rm,admin_menu m
                            WHERE u.Account = '{account}' AND u.ID = ur.UserId AND ur.RoleId = rm.RoleId AND rm.MenuId = m.ID AND m.Type = 4 AND m.PID = @id";
            List<AdminMenu> AdminMenus = (List<AdminMenu>)await DapperSqlHelper.QueryAsync<AdminMenu>(sql,AdminMenu);
            List<string> button = new();
            foreach (var item in AdminMenus)
            {
                button.Add(item.Permission);
                permissionStringList.Add(item.Permission);
            }
            return button;
        }

        public Task<string> GetJwtTokenAsync(List<string> permissions, string uniqueName)
        {
            return Task.Run(() =>
            {
                //可以在Claim中自定义添加用户信息，或权限信息,由方法参数传入
                string iss = PermissionRequirement.Issuer;
                string aud = PermissionRequirement.Audience;
                double expiration = PermissionRequirement.Expiration.TotalSeconds;
                Claim[] claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(expiration)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(expiration).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
                new Claim(ClaimTypes.Name, uniqueName)};
                var now = DateTime.Now;
                foreach (var permission in permissions)
                {
                    claims = claims.Append(new Claim(ClaimTypes.Role, permission)).ToArray();
                }
                var jwt = new JwtSecurityToken(
                    issuer: iss,
                    audience: aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(PermissionRequirement.Expiration),
                    signingCredentials: PermissionRequirement.SigningCredentials
                );
                var encodedJwt = "Bearer " + new JwtSecurityTokenHandler().WriteToken(jwt);
                #region 自定义校验token时间
                long nowSecond = (long)new TimeSpan(DateTime.UtcNow.Ticks).TotalSeconds;
                //Console.WriteLine($"登录时间:{nowSecond}");
                long expiredSecond = (long)(nowSecond + PermissionRequirement.Expiration.TotalSeconds);
                //Console.WriteLine($"TOKEN过期时间:{expiredSecond}");
                _cache.Set(uniqueName, expiredSecond);
                #endregion
                return encodedJwt;
            });
        }

        public async Task<PageQueryRes> LoadUsersAsync(SysUserReq req) {
            int count = await _dbcontext.AdminUsers.CountAsync();
            if (req.PageSize == 0 || req.CurrentPage == 0)
            {//不分页
                List<AdminUser> data = await _dbcontext.AdminUsers.ToListAsync();
                return new PageQueryRes { TotalCount = count, TableData = data };
            }
            else {//分页
                List<AdminUser> data = await _dbcontext.AdminUsers.Skip(req.PageSize * (req.CurrentPage - 1)).Take(req.PageSize).OrderBy(x => x.Id).ToListAsync();
                return new PageQueryRes { TotalCount = count, TableData = data };
            }
        }

        public async Task<PageQueryRes> LoadRolesAsync(SysRoleReq req) {
            if (req.UserId == 0)
            {
                int count = await _dbcontext.AdminRoles.CountAsync();
                if (req.PageSize == 0 || req.CurrentPage == 0)
                {//不分页
                    List<AdminRole> data = await _dbcontext.AdminRoles.ToListAsync();
                    return new PageQueryRes { TotalCount = count, TableData = data };
                }
                else
                {//分页
                    List<AdminRole> data = await _dbcontext.AdminRoles.Skip(req.PageSize * (req.CurrentPage - 1)).Take(req.PageSize).OrderBy(x => x.Id).ToListAsync();
                    return new PageQueryRes { TotalCount = count, TableData = data };
                }
            }
            else {//不分页，且加载单个用户的角色,主要用于授权页面
                string sql = $@"SELECT admin_role.* FROM admin_role,admin_user_role
                            WHERE admin_role.ID = admin_user_role.RoleId AND admin_user_role.UserId = {req.UserId}";
                List<AdminRole> data = (List<AdminRole>)await DapperSqlHelper.QueryAsync<AdminRole>(sql);
                return new PageQueryRes { TotalCount = 0, TableData = data };
            }
        }
        public async Task<bool> AddRoleAsync(AdminRole role) {
            await _dbcontext.AdminRoles.AddAsync(role);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRoleAsync(AdminRole role) {
            var dbEntity = _dbcontext.AdminRoles.Where(_ => _.Id == role.Id).FirstOrDefault();
            dbEntity.Name = role.Name;
            dbEntity.Remark = role.Remark;
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRoleAsync(AdminRole role) {
            var dbEntity = _dbcontext.AdminRoles.Where(_ => _.Id == role.Id).FirstOrDefault();
            var dbRoleMenu = await _dbcontext.AdminRoleMenus.Where(_=>_.RoleId == role.Id).ToListAsync();
            _dbcontext.AdminRoles.Remove(dbEntity);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddUserAsync(AdminUser user)
        {
            await _dbcontext.AdminUsers.AddAsync(user);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(AdminUser user) {
            var dbEntity = _dbcontext.AdminUsers.Where(_ => _.Id == user.Id).FirstOrDefault();
            dbEntity.Password = user.Password;
            dbEntity.NickName = user.NickName;
            dbEntity.Name = user.Name;
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(AdminUser user) {
            var dbEntity = _dbcontext.AdminUsers.Where(_ => _.Id == user.Id).FirstOrDefault();
            var dbUserRole = await _dbcontext.AdminUserRoles.Where(_ => _.UserId == user.Id).ToListAsync();
            _dbcontext.AdminUsers.Remove(dbEntity);
            _dbcontext.RemoveRange(dbUserRole);
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public async Task<Dictionary<string, Object>> LoadRoleCheckBoxAsync(int userId)
        {
            Dictionary<string, Object> keyValues = new();
            string sql = $@"SELECT admin_role.ID
                            FROM admin_role,admin_user_role
                            WHERE admin_role.ID = admin_user_role.RoleId AND admin_user_role.UserId = {userId};
                            SELECT ID,Name,Code,Remark 
                            FROM admin_role";
            //_logger.Info(typeof(SysAdminServiceImpl), "LOAD_ROLE_CHECK_BOX_ASYNC:" + sql);
            var multi = await DapperSqlHelper.QueryMultipleAsync(sql);
            var checkedRoleList = multi.Read<int>().ToList();//必须显示类型读取 否则会生成为 {ID:1} 对象
            var roleList = multi.Read();
            keyValues.Add("checkedRoleList", checkedRoleList);
            keyValues.Add("roleList", roleList);
            return keyValues;
        }

        public async Task<bool> SaveUserRoleAsync(List<int> checkedRoleList, int userId) {
            IDbTransaction transaction = DapperHelper.OpenCurrentDbConnection().BeginTransaction();//开启事务
            try
            {
                string sql1 = $@"DElETE FROM admin_user_role WHERE UserId = {userId};";
                int a = await DapperSqlHelper.ExecuteAsync(sql1,null,transaction);
                string sql2 = $@"INSERT INTO admin_user_role (UserId,RoleId) VALUES (@UserId,@RoleId)";
                List<AdminUserRole> adminUserRoles = new();
                foreach (var roleId in checkedRoleList)
                {
                    adminUserRoles.Add(new AdminUserRole { RoleId = roleId, UserId = userId });
                }
                int b = await DapperSqlHelper.ExecuteAsync(sql2, adminUserRoles, transaction);
                transaction.Commit();
                return (a >= 0) && (b >= 0);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(SysAdminServiceImpl), $"SAVE_USER_ROLE_ERROR:{e}");
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> SaveRoleMenuAsync(List<int> checkedMenuList, int roleId) {
            IDbTransaction transaction = DapperHelper.OpenCurrentDbConnection().BeginTransaction();//开启事务
            try
            {
                string sql1 = $@"DElETE FROM admin_role_menu WHERE RoleId = {roleId};";
                int a = await DapperSqlHelper.ExecuteAsync(sql1, null, transaction);
                string sql2 = $@"INSERT INTO admin_role_menu (RoleId,MenuId) VALUES (@RoleId,@MenuId)";
                List<AdminRoleMenu> adminRoleMenus = new();
                foreach (var menuId in checkedMenuList)
                {
                    adminRoleMenus.Add(new AdminRoleMenu { RoleId = roleId, MenuId = menuId });
                }
                int b = await DapperSqlHelper.ExecuteAsync(sql2, adminRoleMenus, transaction);
                transaction.Commit();
                return (a >= 0) && (b >= 0);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(SysAdminServiceImpl), $"SAVE_USER_ROLE_ERROR:{e}");
                transaction.Rollback();
                return false;
            }
        }
    }
}
