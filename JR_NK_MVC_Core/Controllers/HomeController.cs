using JR_NK_MVC_Core.Common.Enum;
using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Models;
using JR_NK_MVC_Core.Service;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ISysAdminService _adminService;
        private readonly ILoggerHelper _logger;
        public HomeController(ISysAdminService adminService, ILoggerHelper logger) {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpPost("login")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        public async Task<GlobalResponse> Login([FromBody] LoginReq req)
        {
            try
            {
                SysUser user = await _adminService.LoadUser(req.Username, req.Password);
                if (user == null) return GlobalResponse.Of(-1000, "账号密码错误");
                List<PermissionItem> permissionItems = await _adminService.LoadPermissionItems(user);
                if (permissionItems == null) return GlobalResponse.Of(-1,"权限加载失败");
                var tokenJson = await _adminService.GetJwtToken(permissionItems);
                return GlobalResponse.Of(new LoginRes { User = user, PermissionItems = permissionItems, TokenJson = tokenJson });
            }
            catch (Exception e)
            {
                _logger.Error(typeof(HomeController),e.Message);
                return GlobalResponse.Of(-1,e.Message);
            }
        }

        [HttpGet("query")]
        [Authorize("Custom")]
        public GlobalResponse Test() {
            return GlobalResponse.Of();
        }
    }
}
