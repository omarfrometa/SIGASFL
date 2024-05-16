using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SIGASFL.Repositories.Extension;
using SIGASFL.Repositories.Interface;

namespace SIGASFL.Repositories.Implementation
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _context;
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity, bool forceSave = false)
        {
            _context.Set<TEntity>().Add(entity);
            if (forceSave)
            {
                _context.SaveChanges();
            }
        }

        public Task AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().AddAsync(entity);
            return Task.CompletedTask;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _context.Set<TEntity>().Where(expression);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity GetById(params object[] keyValues)
        {
            var entity = _context.Set<TEntity>().Find(keyValues);

            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public void Remove(TEntity entity, bool forceSave = false)
        {
            _context.Set<TEntity>().Remove(entity);
            if (forceSave)
            {
                _context.SaveChanges();
            }
        }
        public void RemoveRange(IEnumerable<TEntity> entities, bool forceSave = false)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            if (forceSave)
            {
                _context.SaveChanges();
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public IQueryable<TEntity> SelectFromStoreProcedure<TRequest>(TRequest request, string storeProcedure)
        {
            return _context
                        .Set<TEntity>()
                        .SelectFromStoreProcedure(request, storeProcedure);
        }


        public IQueryable<TEntity> SelectFromStoreProcedure(string storeProcedure)
        {
            return _context
                        .Set<TEntity>()
                        .SelectFromStoreProcedure(storeProcedure);
        }
    }
}
