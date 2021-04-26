using JR_NK_MVC_Core.Common.Configuration;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Common.until;
using JR_NK_MVC_Core.Models;
using JR_NK_MVC_Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Controllers
{
    /// <summary>
    /// 上传文件控制类
    /// </summary>
    [Route("[controller]")]
    public class UpLoadController : Controller
    {
        private readonly ILoggerHelper _logger;
        private readonly IUploadService _uploadService;
        public UpLoadController(ILoggerHelper logger,IUploadService uploadService)
        {
            _logger = logger;
            _uploadService = uploadService;
        }

        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("onPostUploadBach")]
        public async Task<GlobalResponse> OnPostUploadBachAsync()
        {
            try
            {
                var files = Request.Form.Files;
                List<string> filePaths = await _uploadService.UploadAsync(files);
                return GlobalResponse.Of("上传/导入成功",filePaths);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(UpLoadController), $"MESSAGE:{e.Message} STACKTRACE:{e.StackTrace}");
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("onPostUpload")]
        public async Task<GlobalResponse> OnPostUploadAsync()
        {
            try
            {
                var files = Request.Form.Files;
                string filePath = await _uploadService.UploadAsync(files[0]);
                return GlobalResponse.Of("上传/导入成功", filePath);
            }
            catch (Exception e)
            {
                _logger.Error(typeof(UpLoadController), $"MESSAGE:{e.Message} STACKTRACE:{e.StackTrace}");
                return GlobalResponse.Of(-1, e.Message);
            }
        }

        /// <summary>
        /// 本地版测试只需传入本地文件路径
        /// </summary>
        /// <returns></returns>
        [HttpGet("extlToDataTable")]
        public GlobalResponse ExtlToDataTable(string path)
        {
            try
            {
                var importer = new ExcelImporter();
                IEnumerable<TestModel> models = importer.ExcelToObject<TestModel>(path);
                if (models != null) return GlobalResponse.Of(models);
                return GlobalResponse.Of(-1, "转换失败");
            }
            catch (Exception e)
            {
                _logger.Error(typeof(UpLoadController), $"MESSAGE:{e.Message} STACKTRACE:{e.StackTrace}");
                return GlobalResponse.Of(-1, "转换失败");
            }
        }
    }
}
