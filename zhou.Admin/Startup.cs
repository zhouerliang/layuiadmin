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

            //Session�Ự֧��
            services.AddSingleton<SessionHelper, SessionHelper>();
            services.AddSession();

            //����ʵ�ַ�ʽע��
            services.AddSingleton<ICache, MemoryCache>();
            services.AddMemoryCache();

            //������Ԫע��[ע������������scoped]
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();
            //����ע��ִ�����
            ConfigureRepositoryServices(services);
            //����ע���������
            ConfigureDomainServices(services);

            services.AddMvc().AddJsonOptions(options =>
            {
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //��ʹ���շ���ʽ��key
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //����ʱ���ʽ
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
            //������ʾ�̬�ļ�
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseFileServer();

            //ʹ�û���
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default1", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("Default2", "{area=Code}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        #region �ִ���������� ͳһע��

        //�ִ�ͳһע��
        public void ConfigureRepositoryServices(IServiceCollection services)
        {
            Assembly IRepository = Assembly.Load(new AssemblyName("zhou.Services"));

            //���вִ��ӿ�
            var ITypes = IRepository.GetTypes()
                .Where(c => c.GetTypeInfo().IsInterface && c.Namespace == "zhou.Services.Repository" && typeof(IDependency).IsAssignableFrom(c)).ToList();

            //���вִ�ʵ��
            var Types = IRepository.GetTypes().Where(c => c.Namespace == "zhou.Services.RepositoryImpl").ToList();

            //�ִ���ע���˲ʱ��������
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

        //�������ͳһע��
        public void ConfigureDomainServices(IServiceCollection services)
        {
            //Assembly domain = Assembly.Load(new AssemblyName("SKL.CRM.Domain"));

            ////���вִ��ӿ�
            //var ITypes = domain.GetTypes()
            //    .Where(c => c.GetTypeInfo().IsInterface && c.Namespace == "SKL.CRM.Domain.Services" && typeof(IDependency).IsAssignableFrom(c)).ToList();

            ////���вִ�ʵ��
            //var Types = domain.GetTypes().Where(c => c.Namespace == "SKL.CRM.Domain.ServicesImpl").ToList();

            ////�ִ���ע���˲ʱ��������
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
