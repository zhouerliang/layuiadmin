using Common.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;
using zhou.Services;
using zhou.Services.Adapters;

namespace zhou.Admin
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Session会话支持
            services.AddSingleton<SessionHelper, SessionHelper>();
            services.AddSession();

            //缓存实现方式注入
            services.AddSingleton<ICache, MemoryCache>();
            services.AddMemoryCache();

            //工作单元注入[注意生命周期用scoped]
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();
            //批量注入仓储服务
            ConfigureRepositoryServices(services);
            //批量注入领域服务
            ConfigureDomainServices(services);

            services.AddMvc().AddJsonOptions(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //不使用驼峰样式的key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            //允许访问静态文件
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseFileServer();

            //使用缓存
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default1", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("Default2", "{area=Code}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        #region 仓储、领域服务 统一注入

        //仓储统一注入
        public void ConfigureRepositoryServices(IServiceCollection services)
        {
            Assembly IRepository = Assembly.Load(new AssemblyName("zhou.Services"));

            //所有仓储接口
            var ITypes = IRepository.GetTypes()
                .Where(c => c.GetTypeInfo().IsInterface && c.Namespace == "zhou.Services.Repository" && typeof(IDependency).IsAssignableFrom(c)).ToList();

            //所有仓储实现
            var Types = IRepository.GetTypes().Where(c => c.Namespace == "zhou.Services.RepositoryImpl").ToList();

            //仓储都注册成瞬时生命周期
            Types.ForEach(type =>
            {
                foreach (var itype in ITypes)
                {
                    if (itype.IsAssignableFrom(type))
                    {
                        //services.Add(new ServiceDescriptor(serviceType: itype, implementationType: type, lifetime: ServiceLifetime.Transient));
                        services.AddTransient(itype, type);
                    }
                }
            });
        }

        //领域服务统一注入
        public void ConfigureDomainServices(IServiceCollection services)
        {
            //Assembly domain = Assembly.Load(new AssemblyName("SKL.CRM.Domain"));

            ////所有仓储接口
            //var ITypes = domain.GetTypes()
            //    .Where(c => c.GetTypeInfo().IsInterface && c.Namespace == "SKL.CRM.Domain.Services" && typeof(IDependency).IsAssignableFrom(c)).ToList();

            ////所有仓储实现
            //var Types = domain.GetTypes().Where(c => c.Namespace == "SKL.CRM.Domain.ServicesImpl").ToList();

            ////仓储都注册成瞬时生命周期
            //Types.ForEach(type =>
            //{
            //    foreach (var itype in ITypes)
            //    {
            //        if (itype.IsAssignableFrom(type))
            //        {
            //            //services.Add(new ServiceDescriptor(serviceType: itype, implementationType: type, lifetime: ServiceLifetime.Transient));
            //            services.AddTransient(itype, type);
            //        }
            //    }
            //});

        }

        #endregion
    }
}
