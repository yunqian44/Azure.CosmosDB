using Azure.CosmosDB.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Repository.Implements
{
    /// <summary>
    /// 基类仓储
    /// </summary>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class,new()
    {
        protected DbContext Db;

        public async virtual Task<TEntity> GetById(string partitionKey)
        {
            var props = typeof(TEntity).GetProperties();
            var s = new TEntity();
             

            
            return await Db.Set<TEntity>().FindAsync( partitionKey, "");
        }

        public async virtual Task<TEntity> Add(TEntity entity)
        {
            await Db.AddAsync<TEntity>(entity);
            return entity;
        }

        public virtual bool Update(TEntity entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public virtual bool Remove(TEntity entity)
        {
            Db.Set<TEntity>().Remove(entity);
            return true;

        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Db.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> conditions)
        {
            return Db.Set<TEntity>().Where(conditions);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
