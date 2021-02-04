using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Get all sql rows in the DbSet.
        /// AsNoTracking enabled.
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<T> FindAll();
        /// <summary>
        /// Finds all sql rows adhering to a condition specified.
        /// </summary>
        /// <param name="expression">Condition to run</param>
        /// <returns>IQueryable</returns>
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Finds and returns all SQL rows from a set that SQL finds via LIKE.
        /// </summary>
        /// <param name="model">Model name.</param>
        /// <param name="property">Property name to aply LIKE statement on.</param>
        /// <param name="query">The value property must be LIKE.</param>
        /// <returns></returns>
        IQueryable<T> FindBySqlLike(string model, string property, string query);
        /// <summary>
        /// Find a SQL row by primarykey.
        /// </summary>
        /// <param name="key">Key object.</param>
        /// <returns>Row object or null</returns>
        Task<T> FindByPrimaryKey(object key);
        /// <summary>
        /// Find a SQL row by primarykeys.
        /// Used for tables with composite keys
        /// </summary>
        /// <param name="keyValues">Key objects. MUST BE IN CORRECT ORDER.</param>
        /// <returns>Row object or null</returns>
        Task<T> FindByPrimaryKeys(params object[] keyValues);
        /// <summary>
        /// Add entity to context.
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);
        /// <summary>
        /// Update entity in the context.
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// Removes the entity from the context.
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
    }
}
