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
            // ���������ڽ��������������� Microsoft.Extensions.DependencyInjection.IServiceScope��
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var env = services.GetRequiredService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    try
                    {
                        // �� system.IServicec�ṩ�����ȡ T ���͵ķ���
                        var context = services.GetRequiredService<UserContext>();
                        
                        context.Database.EnsureDeleted();
                        Console.WriteLine("**********��ʼ��ʼ������**********");
                        context.Database.EnsureCreated();

                    }
                    catch (Exception e)
                    {
                        var logger = loggerFactory.CreateLogger<Program>();
                        logger.LogError(e, "Error occured seeding the Database.");
                    }
                }
            }

            // ���� web Ӧ�ó�����ֹ�����߳�, ֱ�������رա�
            // ������ WebHost ֮�󣬱�������� Run �������� Run ������ȥ���� WebHost �� StartAsync ����
            // ��Initialize����������Application�ܵ������Թ�������Ϣ
            // ִ��HostedServiceExecutor.StartAsync����

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
