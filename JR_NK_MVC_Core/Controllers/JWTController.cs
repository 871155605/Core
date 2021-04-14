using JR_NK_MVC_Core.Common.JWT;
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
    public class JWTController : Controller
    {

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        [HttpGet("getToken")]
        public dynamic GetToken()
        {
            return JwtToken.BuildJwtToken();
        }

        /// <summary>
        /// 测试获取令牌是否成功
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        [Authorize("Custom")]
        public IActionResult Test()
        {
            return Ok("成功");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        [Authorize("Custom")]
        public IActionResult LoginOut()
        {
            return Ok("退出失败");
        }

        /// <summary>
        /// 401
        /// </summary>
        /// <returns></returns>
        [HttpGet("denied")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Denied()
        {
            return View();
        }

        /// <summary>
        /// 404
        /// </summary>
        /// <returns></returns>
        [HttpGet("notFoundPage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult NotFoundPage()
        {
            return View();
        }

        /// <summary>
        /// 500
        /// </summary>
        /// <returns></returns>
        [HttpGet("errorPage")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
