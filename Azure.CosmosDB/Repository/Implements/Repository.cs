using Azure.CosmosDB.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization.Internal;
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
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext Db;

        protected readonly DbSet<TEntity> DbSet;

        public abstract void GetDb();//抽象方法，子类必须实现

        public Repository(DbContext dbContext)
        {
            GetDb();//抽象方法
            DbSet = Db.Set<TEntity>();
        }

        public async virtual Task<TEntity> GetById(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async virtual Task<TEntity> Add(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public virtual bool Update(TEntity entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public virtual bool Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            return true;

        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> conditions)
        {
            return DbSet.Where(conditions);
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
