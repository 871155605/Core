using JR_NK_MVC_Core.Common.Cache;
using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public async Task<SysUser> LoadUser(string account, string password)
        {
 
            List<SysUser> users = await _dbcontext.SysUsers.Where(_ => _.Account == account && _.Password == password).ToListAsync();
            SysUser user = null;
            if (users != null && users.Count > 0)
            {
                user = users[0];
                _cache.Set(user.Account, user);
                _logger.Info(typeof(SysAdminServiceImpl), $"ADD USER TO CACHE.USERID:{user.Account}");
            }
            return user;
        }

        public async Task<List<PermissionItem>> LoadPermissionItems(SysUser user)
        {
            List<PermissionItem> permissionItems = new List<PermissionItem>();
            if (user == null)
            {//加载所有权限与菜单
                List<SysUserRole> urs = await _dbcontext.SysUserRoles.Include(user_role => user_role.SysRole).ToListAsync();
                permissionItems = await LoadPermissionItemsByURs(urs);
                _cache.Set("AllPERMISSIONS", permissionItems);
                _logger.Info(typeof(SysAdminServiceImpl), $"ADD ALL PERMISSION LIST TO CACHE. COUNT {permissionItems.Count}");
            }
            else
            {//加载用户的权限与菜单  因为在登录时加载 所以无需缓存
                List<SysUserRole> urs = await _dbcontext.SysUserRoles.Where(user_role => user_role.SysUserId == user.Id).Include(user_role => user_role.SysRole).ToListAsync();
                permissionItems = await LoadPermissionItemsByURs(urs);
                //_cache.Set(user.Account, permissionItems);
                //_logger.Info(typeof(SysAdminServiceImpl), $"ADD USER:{user.Account} PERMISSION LIST TO CACHE. COUNT {permissionItems.Count}");
            }
            return permissionItems;
        }

        private async Task<List<PermissionItem>> LoadPermissionItemsByURs(List<SysUserRole> urs) {
            List<PermissionItem> permissionItems = new List<PermissionItem>();
            foreach (var ur in urs)
            {
                List<SysRoleMenu> rms = await _dbcontext.SysRoleMenus.Where(_ => _.SysRoleId == ur.SysRoleId).Include(role_menu => role_menu.SysMenu).Include(role_menu => role_menu.SysRole).ToListAsync();
                var list = (from item in rms
                            orderby item.SysRoleId
                            select new PermissionItem
                            {
                                Link = item.SysMenu?.Link,
                                Pid = item.SysMenu?.Pid,
                                Permission = item.SysMenu?.Permission,
                                Role = item.SysRole?.Name,
                            }).ToList();
                permissionItems.InsertRange(permissionItems.Count, list);
            }
            return permissionItems;
        }

        public Task<string> GetJwtToken(List<PermissionItem> permissions)
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
                new Claim(JwtRegisteredClaimNames.Aud,aud)};
                var now = DateTime.Now;
                foreach (var permissionItem in permissions)
                {
                    claims = claims.Append(new Claim(ClaimTypes.Role, permissionItem.Permission)).ToArray();
                    Console.WriteLine(permissionItem.Permission);
                    Console.WriteLine(claims.Length);
                }
                var jwt = new JwtSecurityToken(
                    issuer: iss,
                    audience: aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(PermissionRequirement.Expiration),
                    signingCredentials: PermissionRequirement.SigningCredentials
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return "Bearer " + encodedJwt;
            });
        }

        public async Task<bool> AddUser(SysUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserRoles(List<SysUserRole> urs)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddRole(SysRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddRoleMenus(List<SysRoleMenu> rms)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddMenu(SysMenu menu)
        {
            throw new NotImplementedException();
        }
    }
}
