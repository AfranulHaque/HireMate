using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HireMate.DataManagement.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly HireMateDBContext Context;
        private readonly DbSet<TEntity> _entities;

        public Repository(HireMateDBContext context)
        {

            Context = context;
            _entities = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Data => _entities;

        #region synchronous
        public TEntity GetById(TKey id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities;
        }

        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return ApplyIncludesOnQuery(_entities, includeProperties);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        private IEnumerable<IEnumerable<TSource>> ChunkDataInternal<TSource>(IQueryable<TSource> source, int chunkSize)
        {
            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }
        public IEnumerable<TEntity> FindInChunk<TSortedBy>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSortedBy>> orderby, int chunkSize = 500, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.Where(predicate);

            if (includeProperties != null)
            {
                query = ApplyIncludesOnQuery(query, includeProperties);
            }
            query = query.OrderBy(orderby);

            return ChunkDataInternal(query, chunkSize).SelectMany(_ => _);

        }
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }

            if (includeProperties != null)
            {
                query = ApplyIncludesOnQuery(query, includeProperties);
            }

            return query;
        }

        public IQueryable<TEntity> ApplyIncludesOnQuery(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return (includeProperties.Aggregate(query, (current, include) => current.Include(include)));
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void Remove<TOEntity>(TOEntity entity) where TOEntity : class
        {
            GetEntity<TOEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {

            _entities.RemoveRange(entities);
        }

        public void RemoveRangeOther<TOEntity>(IEnumerable<TOEntity> entities) where TOEntity : class

        {
            GetEntity<TOEntity>().RemoveRange(entities);
        }

        public void Attach(TEntity entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
                _entities.Attach(entity);

        }

        public void AttachEntity<TOEntity>(TOEntity otherEntity) where TOEntity : class
        {
            var entry = Context.Entry(otherEntity);
            if (entry.State == EntityState.Detached)
                Context.Set<TOEntity>().Attach(otherEntity);

        }

        public void UpdateMinimal<TAnotherEntity>(
            TAnotherEntity entity,
            params Expression<Func<TAnotherEntity, object>>[] propsToBeUpdated) where TAnotherEntity : class
        {

            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                Context.Set<TAnotherEntity>().Attach(entity);
            }

            if (propsToBeUpdated.Length > 0)
            {
                foreach (var property in propsToBeUpdated)
                {
                    entry.Property(property).IsModified = true;
                }
            }
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propsToBeExcluded)
        {
            Attach(entity);
            var entry = Context.Entry(entity);
            entry.State = EntityState.Modified;

            if (propsToBeExcluded.Length > 0)
            {
                foreach (var property in propsToBeExcluded)
                {
                    entry.Property(property).IsModified = false;
                }
            }
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public IEnumerable<IEnumerable<TSource>> ChunkData<TSource>(IQueryable<TSource> source, int chunkSize)
        {
            for (int i = 0; i < source.Count(); i += chunkSize)
                yield return source.Skip(i).Take(chunkSize);
        }

        public DbSet<TOther> GetEntity<TOther>() where TOther : class
        {
            return Context.Set<TOther>();
        }

        public void ExecuteRawSql(string sql, params object[] parameters)
        {
            if (parameters?.Length == 0)
            {
                Context.Database.ExecuteSqlRaw(sql);
                return;
            }

            Context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        #endregion

        #region  asynchronous
        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            if (parameters?.Length == 0)
            {
                await Context.Database.ExecuteSqlRawAsync(sql);
                return;
            }

            await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }


        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        #endregion
    }
}
