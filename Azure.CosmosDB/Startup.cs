using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.CosmosDB.Common;
using Azure.CosmosDB.Context;
using Azure.CosmosDB.Repository.Implements;
using Azure.CosmosDB.Repository.Interface;
using Azure.CosmosDB.Service;
using Azure.CosmosDB.Service.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Azure.CosmosDB
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Env.ContentRootPath));

            services.AddDbContext<UserContext>(options => options.UseCosmos(Appsettings.app("CosmosDB", "Endpoint"), Appsettings.app("CosmosDB", "Key"), Appsettings.app("CosmosDB", "DataBase"), cosmosOptionsAction => cosmosOptionsAction.Region(Appsettings.app("CosmosDB", "region"))));

            // ע�� Ӧ�ò�Application
            services.AddScoped<IUserService, UserService>();

            // ע�� ������ʩ�� - ���ݲ�
            services.AddScoped<IUserRepository, UserRepository>();

            //��ӷ���
            services.AddAutoMapper(typeof(AutoMapperConfig));
            //��������
            AutoMapperConfig.RegisterMappings();


            services.AddControllersWithViews();

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}
