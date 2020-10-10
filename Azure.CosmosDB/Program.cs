using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.CosmosDB.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Azure.CosmosDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // 创建可用于解析作用域服务的新 Microsoft.Extensions.DependencyInjection.IServiceScope。
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    try
                    {
                        // 从 system.IServicec提供程序获取 T 类型的服务。
                        var context = services.GetRequiredService<UserContext>();
                        
                        context.Database.EnsureDeleted();
                        Console.WriteLine("**********开始初始化数据**********");
                        context.Database.EnsureCreated();

                    }
                    catch (Exception e)
                    {
                        var logger = loggerFactory.CreateLogger<Program>();
                        logger.LogError(e, "Error occured seeding the Database.");
                    }
                }
            }

            // 运行 web 应用程序并阻止调用线程, 直到主机关闭。
            // 创建完 WebHost 之后，便调用它的 Run 方法，而 Run 方法会去调用 WebHost 的 StartAsync 方法
            // 将Initialize方法创建的Application管道传入以供处理消息
            // 执行HostedServiceExecutor.StartAsync方法

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls("http://*:9001")
                    .UseStartup<Startup>();
                });
    }
}
