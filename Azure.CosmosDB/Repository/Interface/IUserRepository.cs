using Azure.CosmosDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Repository.Interface
{
    /// <summary>
    /// IUserRepository接口
    /// </summary>
    public interface IUserRepository : IRepository<UserModel>
    {
        //一些UserModel独有的接口
        Task<UserModel> GetByName(string name);
    }
}
