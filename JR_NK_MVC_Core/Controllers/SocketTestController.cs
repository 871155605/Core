using JR_NK_MVC_Core.Common.Socket;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SocketTestController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// 测试接收消息
        /// </summary>
        /// <param name="socketId"></param>
        /// <returns></returns>
        [HttpGet("receive")]
        public void ReceiveMessage(string socketId)
        {

        }

        /// <summary>
        /// 测试发送消息给前端
        /// </summary>
        /// <param name="socketId"></param>
        /// <returns></returns>
        [HttpGet("send")]
        public IActionResult SenMessage(string socketId)
        {
            return Ok();
        }

    }
}
