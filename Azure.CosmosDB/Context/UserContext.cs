using Azure.CosmosDB.Common;
using Azure.CosmosDB.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }

        /// <summary>
        /// 重写连接数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 从 appsetting.json 中获取配置信息
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // 定义要使用的数据库
            optionsBuilder.UseCosmos(Appsettings.app("CosmosDB", "Endpoint"), Appsettings.app("CosmosDB", "Key"), Appsettings.app("CosmosDB", "DataBase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //配置分区键 
            modelBuilder.Entity<UserModel>()
                .HasPartitionKey(o => o.PartitionKey)
                .HasKey(o => o.PartitionKey);
                

            //调用EnsureCreated方法只针对与添加数据有效，但是数据库如果有数据的话，
            //也就是对数据更改将无效
            Console.WriteLine("**********Blog表开始初始化数据**********");
            #region 数据库数据映射
            modelBuilder.Entity<UserModel>().HasData(
                   new UserModel { PartitionKey = "1", Id = 1, Name = "张无忌", Age = 12, Address = "北京市西城区鲍家街43号", Remark = "中央音乐学院" },
                   new UserModel { PartitionKey = "2", Id = 2, Name = "令狐冲", Age = 20, Address = "佛山市南海区灯湖东路6号", Remark = "广发商学院" });

            #endregion
        }

    }
}
