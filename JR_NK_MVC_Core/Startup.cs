using JR_NK_MVC_Core.Common.Cache;
using JR_NK_MVC_Core.Common.Configuration;
using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using JR_NK_MVC_Core.Common.Socket;
using JR_NK_MVC_Core.Common.Until;
using JR_NK_MVC_Core.Entities;
using JR_NK_MVC_Core.Service;
using JR_NK_MVC_Core.Service.Impl;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core
{
    public class Startup
    {
        private readonly string ApiName = "JR_NK_MVC_Core";
        public static ILoggerRepository LogRepository { get; set; }
        public IConfiguration Configuration { get; }

        public static readonly ILoggerFactory sqlLogger= LoggerFactory.Create(builder => { builder.AddConsole(); });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogRepository = LogManager.CreateRepository(ApiName);
            XmlConfigurator.Configure(LogRepository, new FileInfo("Log4net.config"));//ָ�������ļ���
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)throw new ArgumentNullException(nameof(services));
            services.AddControllersWithViews(option => { option.Filters.Add(typeof(WebSocketFilter)); });
            #region ע��������Ϣ
            services.AddOptions();
            services.Configure<CacheOptions>(Configuration.GetSection("Cache"));
            services.Configure<UploadOptions>(Configuration.GetSection("UpLoad"));
            //��ȡJWR���ò�����PermissionRequirement
            PermissionRequirement permissionRequirement =  SetPermissionRequirement();
            services.AddDbContext<JRDBContext>(options =>
            {
                options.UseSqlServer(Configuration["SqlServerconStr"]);//.UseLoggerFactory(sqlLogger)����EFCORE-SQL��־
                DapperSqlHelper.ConnectionStr = Configuration["SqlServerconStr"];
            });
            #endregion
            #region ע�����
            services.AddSingleton<ILoggerHelper, LogHelper>();
            //services.AddSingleton<ICache, RedisCache>();
            services.AddSingleton<ICache, DictionaryCache>();
            services.AddScoped<ISysAdminService,SysAdminServiceImpl>();
            services.AddScoped<IUploadService, UploadServiceImpl>();
            #endregion
            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    // {ApiName} �����ȫ�ֱ����������޸�
                    Version = "V1",
                    Title = $"{ApiName} �ӿ��ĵ�����Netcore 3.0",
                    Description = $"{ApiName} HTTP API V1",
                });
                c.OrderActionsBy(o => o.RelativePath);
                // ��ȡxmlע���ļ���Ŀ¼
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{ApiName}.xml");
                c.IncludeXmlComments(xmlPath, false);
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                #region Token�󶨵�ConfigureServices
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion
            });
            #endregion
            #region ������ı�������
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
            #endregion
            //������ע���������������ṩIHttpContextAccessor
            services.AddHttpContextAccessor();
            //JWT��Ȩ��֤
            services.AddAuthorizationSetup(permissionRequirement);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName}.Core V1");
                c.RoutePrefix = "swagger";
            });
            #endregion

            #region WebSocket
            app.UseWebSockets();
            //app.UseMiddleware<WebSocketManagerMiddleware>();
            #endregion

            #region ���������תҳ������ ��Ϊǰ�˴���
            /*app.UseStatusCodePages(async context =>
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;
                //401
                if (response.StatusCode == (int)HttpStatusCode.Unauthorized) response.Redirect("/pages/home/login.html");
                //403
                if (response.StatusCode == (int)HttpStatusCode.Forbidden) response.Redirect("/jwt/Denied");
                //404
                if (response.StatusCode == (int)HttpStatusCode.NotFound) response.Redirect("/jwt/notFoundPage");
                //500
                if (response.StatusCode == (int)HttpStatusCode.NotFound) response.Redirect("/jwt/errorPage");
            });*/
            #endregion

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            #region ����·��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            #endregion
        }

        /// <summary>
        /// Ȩ������
        /// </summary>
        /// <returns></returns>
        private PermissionRequirement SetPermissionRequirement() {
            JWTConfig config = Configuration.GetSection("JwtSetting").Get<JWTConfig>();
            PermissionRequirement.DeniedAction = config.DeniedAction;
            PermissionRequirement.LoginPath = config.LoginPath;
            PermissionRequirement.Issuer = config.Issuer;
            PermissionRequirement.Audience = config.Audience;
            PermissionRequirement.Expiration = TimeSpan.FromSeconds(config.Expiration);
            PermissionRequirement.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey)), SecurityAlgorithms.HmacSha256);
            PermissionRequirement permissionRequirement = new PermissionRequirement();
            return permissionRequirement;
        }
    }
}
