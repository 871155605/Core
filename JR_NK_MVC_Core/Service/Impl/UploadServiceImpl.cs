using JR_NK_MVC_Core.Common.Configuration;
using JR_NK_MVC_Core.Common.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Service.Impl
{
    public class UploadServiceImpl : IUploadService
    {
        private readonly ILoggerHelper _logger;
        private readonly string _import_path;
        private readonly string _path;
        public UploadServiceImpl(ILoggerHelper logger, IOptions<UploadOptions> oUploadOptions)
        {
            _logger = logger;
            _import_path = oUploadOptions.Value.ImportPath;
            _path = oUploadOptions.Value.Path;
        }

        //有BUG
        public async Task<List<string>> UploadAsync(IFormFileCollection files)
        {
            List<string> filePaths = new();
            if (files == null) throw new Exception("FILES IS NULL");
            foreach (var file in files)
            {
                string filePath = await UploadAsync(file);
                filePaths.Add(filePath);
                using var stream = System.IO.File.Create(filePath);
                await file.CopyToAsync(stream);
            }
            return filePaths;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            //string suffix = file.FileName.Split(".")[1];//获取后缀名
            if (file == null) throw new Exception("FILE IS NULL");
            var filePath = $"{_path}{DateTime.Now.Ticks}-{file.FileName}";
            using (var stream = System.IO.File.Create(filePath))
            await file.CopyToAsync(stream);
            return file.FileName;
        }
    }
}
