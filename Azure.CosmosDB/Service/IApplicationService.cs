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
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="viewmodel"></param>
        void Update(T viewmodel);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
    }
}
