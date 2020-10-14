using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// 根据Id获取对象
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<TEntity> GetById(string partitionKey);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        bool Update(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        bool Remove(TEntity entity);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 查询根据条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> conditions);


        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
