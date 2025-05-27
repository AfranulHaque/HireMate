using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HireMate.DataManagement.Repositories
{
    public interface IRepository<TEntity, Tkey> where TEntity : class
    {
        TEntity GetById(Tkey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        void Attach(TEntity entity);

        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propsToBeExcluded);
        void UpdateMinimal<TAnotherEntity>(TAnotherEntity entity, params Expression<Func<TAnotherEntity, object>>[] propsToBeUpdated) where TAnotherEntity : class;

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Remove<TOEntity>(TOEntity entity) where TOEntity : class;
        void RemoveRangeOther<TOEntity>(IEnumerable<TOEntity> entities) where TOEntity : class;
        void AttachEntity<TOEntity>(TOEntity otherEntity) where TOEntity : class;
        void ExecuteRawSql(string sql, params object[] parameters);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> FindInChunk<TSortedBy>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TSortedBy>> orderBy, int chunkSize = 500, params Expression<Func<TEntity, object>>[] includeProperties);
        void SaveChanges();


        Task<TEntity> GetByIdAsync(Tkey id);
        Task ExecuteRawSqlAsync(string sql, params object[] parameters);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task SaveChangesAsync();
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
