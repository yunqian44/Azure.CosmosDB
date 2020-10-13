using Azure.CosmosDB.Factory;
using Azure.CosmosDB.Models;
using Azure.CosmosDB.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Repository.Implements
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {

        public override void GetDb()
        {
            Db = AbstractDbFactory.GetUserCurrentDbContext();
        }

        #region 01，获取用户根据姓名+async Task<UserModel> GetByName(string name)
        /// <summary>
        /// 获取用户根据姓名
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public async Task<UserModel> GetByName(string name)
        {
            //返回一个新查询，其中返回的实体将不会在 System.Data.Entity.DbContext 中进行缓存
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
        }
        #endregion
    }
}
