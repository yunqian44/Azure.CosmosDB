using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Service
{
    public interface IApplicationService<T> where T : class, new()
    {
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        T GetById(string partitionKey);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="viewmodel"></param>
        Task<int> Update(T viewmodel);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="partitionKey"></param>
        void Remove(string partitionKey);
    }
}
