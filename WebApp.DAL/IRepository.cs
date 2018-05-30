using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    public interface IRepository<T> where T : class, IDisposable
    {
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="Entity">The Entity.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<TEntity>() where TEntity: class;

        /// <summary>
        /// Gets the specified order by.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>        
        /// <returns></returns>
        IEnumerable<T> Get<TEntity,TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex,
            int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;

        /// <summary>
        /// Gets the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <typeparam name="TOrderBy">The type of the order by.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        IEnumerable<T> Get<TEntity,TOrderBy>(Expression<Func<T, bool>> criteria,
            Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending) where TEntity : class;


        /// <summary>
        /// Gets one Entity based on matching criteria
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        T Single<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Firsts the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T First<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class;

        T FirstOrDefault<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class;

        Task<T> FirstOrDefaultAsync<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Finds entities based on provided criteria.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        IEnumerable<T> Find<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Finds one Entity based on provided criteria.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        T FindOne<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class;

        T GetById(int id);

        /// <summary>
        /// Gets Entity by key.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="keyValue">The key value.</param>
        /// <returns></returns>
        T GetByKey<TEntity>(object keyValue) where TEntity : class;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <returns></returns>
        IQueryable<T> GetQuery<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> GetQuery<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class;

        /// <summary>
        /// Attaches the specified Entity.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="Entity">The Entity.</param>
        void Attach<TEntity>(TEntity Entity) where TEntity : class;

        void Add(T Entity);

        void Update(T Entity);

        void Delete(T Entity);

        void Delete(int id);

        /// <summary>
        /// Deletes the specified Entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the Entity.</typeparam>
        /// <param name="Entity">The Entity.</param>
        void Delete<TEntity>(T Entity) where TEntity : class;

        /// <summary>
        /// Deletes one or many entities matching the specified criteria
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        void Delete<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class;

        /// <summary>
        /// Counts the specified entities.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <returns></returns>
        int Count<TEntity>() where TEntity : class;

        Task<int> CountAsync<TEntity>() where TEntity : class;

        /// <summary>
        /// Counts entities with the specified criteria.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        int Count<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class;
    }
}
