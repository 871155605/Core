using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Service
{
    public interface IUploadService
    {
        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public Task<List<string>> UploadAsync(IFormFileCollection files);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Task<string> UploadAsync(IFormFile file);
    }
}
