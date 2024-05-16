using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIGASFL.Repositories.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity GetById(params object[] keyValues);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void Add(TEntity entity, bool forceSave = false);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity, bool forceSave = false);
        void RemoveRange(IEnumerable<TEntity> entities, bool forceSave = false);
        void Update(TEntity entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IQueryable<TEntity> SelectFromStoreProcedure<TRequest>(TRequest request, string storeProcedure);
        IQueryable<TEntity> SelectFromStoreProcedure(string storeProcedure);
    }
}