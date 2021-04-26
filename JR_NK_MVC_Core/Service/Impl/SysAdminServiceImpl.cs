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

        public async Task<SysUser> LoadUserAsync(string account, string password)
        {
            List<SysUser> users = await _dbcontext.SysUsers.Where(_ => _.Account == account && _.Password == password).ToListAsync();
            SysUser user = null;
            if (users != null && users.Count > 0)
            {
                user = users[0];
                _cache.Set(user.Account, user);
                _logger.Info(typeof(SysAdminServiceImpl), $"ADD USER TO CACHE.USERID:{user.Account}");
            }
            //SysUser user = await DapperSqlHelper.QueryFirstOrDefaultAsync<SysUser>($"SELECT * FROM sys_user WHERE Account = '{account}' And Password = '{password}';");
            return user;
        }
        public async Task<Dictionary<string,Object>> LoadUserPermissionMenusAsync(string account) {
            Dictionary<string, Object> keyValues = new Dictionary<string, object>();
            List<string> permissionStringList = new List<string>();
            List<PermissionMenu> permissionMenuList = new List<PermissionMenu>();
            keyValues.Add("menus",permissionMenuList);
            keyValues.Add("permissions", permissionStringList);
            string sql1 = $@"SELECT sys_menu.* FROM sys_user,sys_role,sys_role_menu,sys_menu
                    WHERE sys_role.ID = sys_role_menu.SysRoleId AND sys_role_menu.SysMenuId = sys_menu.ID AND sys_user.Account = '{account}' AND Type = 1;";
            _logger.Info(typeof(SysAdminServiceImpl),sql1);
            List<SysMenu> sysMenus1 = (List<SysMenu>) await DapperSqlHelper.QueryAsync<SysMenu>(sql1);
            foreach (var item1 in sysMenus1)
            {
                PermissionMenu permission1 = new PermissionMenu { IconCls = item1.Icon, Name = item1.Name,Path= item1.Link, Code = item1.Code };
                permissionStringList.Add(item1.Permission);
                permissionMenuList.Add(permission1);
                string sql2 = $"SELECT * FROM sys_menu WHERE PID = {item1.Id} AND Type = 2;";
                _logger.Info(typeof(SysAdminServiceImpl), sql2);
                List<SysMenu> sysMenus2 = (List<SysMenu>)await DapperSqlHelper.QueryAsync<SysMenu>(sql2);
                if (sysMenus2 != null && sysMenus2.Count > 0)
                {
                    List<PermissionMenu> Children1 = new List<PermissionMenu>();
                    permission1.Children = Children1;
                    foreach (var item2 in sysMenus2)
                    {
                        permissionStringList.Add(item2.Permission);
                        PermissionMenu permission2 = new PermissionMenu { IconCls = item2.Icon, Name = item2.Name, Path = item2.Link,Code=item2.Code };
                        Children1.Add(permission2);
                        string sql3 = $"SELECT * FROM sys_menu WHERE PID = {item2.Id} AND Type = 3;";
                        _logger.Info(typeof(SysAdminServiceImpl), sql3);
                        List<SysMenu> sysMenus3 = (List<SysMenu>)await DapperSqlHelper.QueryAsync<SysMenu>(sql3);
                        if (sysMenus3 != null && sysMenus3.Count > 0)
                        {
                            List<PermissionMenu> Children2 = new List<PermissionMenu>();
                            permission2.Children = Children2;
                            foreach (var item3 in sysMenus3)
                            {
                                permissionStringList.Add(item3.Permission);
                                PermissionMenu permission3 = new PermissionMenu { IconCls = item3.Icon, Name = item3.Name, Path = item3.Link, Code = item3.Code };
                                Children2.Add(permission3);
                                List<string> button3 = await LoadPermissionButtonAsync(item3,permissionStringList);
                                permission3.Button = button3;
                            }
                        }
                        else {
                            List<string> button2 = await LoadPermissionButtonAsync(item2,permissionStringList);
                            permission2.Button = button2;
                        }
                    }
                }
                else {
                    List<string> button1 = await LoadPermissionButtonAsync(item1,permissionStringList);
                    permission1.Button = button1;
                }
            }
            return keyValues;
        }

        private async Task<List<string>> LoadPermissionButtonAsync(SysMenu sysMenu,List<string> permissionStringList)
        {
            string sql = "SELECT * FROM sys_menu WHERE pid = @id AND type = 4";
            List<SysMenu> sysMenus = (List<SysMenu>)await DapperSqlHelper.QueryAsync<SysMenu>(sql,sysMenu);
            List<string> button = new List<string>();
            foreach (var item in sysMenus)
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
                #region 自定义校验token时间时使用
                long nowSecond = (long)new TimeSpan(DateTime.UtcNow.Ticks).TotalSeconds;
                Console.WriteLine($"登录时间:{nowSecond}");
                long expiredSecond = (long)(nowSecond + PermissionRequirement.Expiration.TotalSeconds);
                Console.WriteLine($"TOKEN过期时间:{expiredSecond}");
                _cache.Set(uniqueName, expiredSecond);
                #endregion
                return encodedJwt;
            });
        }

        public async Task<PageQueryRes> LoadUsersAsync(SysUserReq req) {
            List<Expression<Func<SysUser, bool>>> where = new List<Expression<Func<SysUser, bool>>>{x => 1 == 1};
            if (!req.Name.IsNullOrEmpty()) where.Add(x => x.Name.Contains(req.Name));
            if (!req.NickName.IsNullOrEmpty()) where.Add(x => x.NickName.Contains(req.NickName));
            IQueryable<SysUser> query = null;
            foreach (var w in where)
            {
                query = _dbcontext.SysUsers.Where(w);
            }
            int count = await query.CountAsync();
            List<SysUser> data = await query.Skip(req.PageSize * (req.CurrentPage - 1)).Take(req.PageSize).OrderBy(x => x.Id).ToListAsync();
            return new PageQueryRes {TotalCount = count,TableData = data};
        }

        public async Task<bool> AddUserAsync(SysUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserRolesAsync(List<SysUserRole> urs)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddRoleAsync(SysRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddRoleMenusAsync(List<SysRoleMenu> rms)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddMenuAsync(SysMenu menu)
        {
            throw new NotImplementedException();
        }
    }
}
