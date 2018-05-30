using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.DAL
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of Entity for this Repository.</typeparam>
    public class MyRepository<T> : IRepository<T> where T : class, IDisposable
    {        
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MyRepository<T>"/> class.
        /// </summary>
        public MyRepository()
        {

        }

        public MyRepository(DbContext DbContext)
        {
            if (DbContext == null)
                throw new ArgumentNullException("Null DbContext");
            this.DbContext = DbContext;
            DbSet = this.DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public void Add(T Entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(Entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(Entity);
            }

            CancellationToken _CancellationToken = new CancellationToken();
            DbContext.SaveChangesAsync(_CancellationToken);
        }

        public void Attach<TEntity>(TEntity Entity) where TEntity : class
        {
            if (Entity == null)
            {
                throw new ArgumentNullException("Entity");
            }

            DbContext.Set<TEntity>().Attach(Entity);
        }

        public int Count<TEntity>() where TEntity : class
        {
            return GetQuery<T>().Count();
        }

        public int Count<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class
        {
            return GetQuery<T>().Count(criteria);
        }

        public Task<int> CountAsync<TEntity>() where TEntity : class
        {
            return GetQuery<T>().CountAsync();
        }

        public void Delete(int id)
        {
            var Entity = GetById(id);
            if (Entity == null) return; // not found; assume already deleted.
            Delete(Entity);
        }

        public void Delete(T Entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(Entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(Entity);
                DbSet.Remove(Entity);
            }

            CancellationToken _CancellationToken = new CancellationToken();
            DbContext.SaveChangesAsync(_CancellationToken);
        }

        public void Delete<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class
        {
            IEnumerable<T> records = Find<T>(criteria);

            foreach (T record in records)
            {
                Delete(record);
            }
        }

        public void Delete<TEntity>(T Entity) where TEntity : class
        {
            if (Entity == null)
            {
                throw new ArgumentNullException("Entity");
            }
            DbContext.Set<T>().Remove(Entity);
        }

        public IEnumerable<T> Find<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class
        {
            return GetQuery<T>().Where(criteria);
        }

        public T FindOne<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class
        {
            return GetQuery<T>().Where(criteria).FirstOrDefault();
        }

        public T First<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class
        {
            return GetQuery<T>().First(predicate);
        }

        public T FirstOrDefault<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class
        {
            return GetQuery<T>().FirstOrDefault(predicate);
        }

        public Task<T> FirstOrDefaultAsync<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class
        {
            return GetQuery<T>().FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<T> Get<TEntity, TOrderBy>(Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<T>()
                    .OrderBy(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
            }
            return
                GetQuery<T>()
                    .OrderByDescending(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
        }

        public IEnumerable<T> Get<TEntity, TOrderBy>(Expression<Func<T, bool>> criteria, Expression<Func<T, TOrderBy>> orderBy, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending) where TEntity : class
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return GetQuery<T>(criteria).
                    OrderBy(orderBy).
                    Skip((pageIndex - 1) * pageSize).
                    Take(pageSize)
                    .AsEnumerable();
            }
            return
                GetQuery<T>(criteria)
                    .OrderByDescending(orderBy)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .AsEnumerable();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public IEnumerable<T> GetAll<TEntity>() where TEntity : class
        {
            return GetQuery<T>().AsEnumerable();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public T GetByKey<TEntity>(object keyValue) where TEntity : class
        {
            EntityKey key = GetEntityKey<T>(keyValue);

            object originalItem;
            if (((IObjectContextAdapter)DbContext).ObjectContext.TryGetObjectByKey(key, out originalItem))
            {
                return (T)originalItem;
            }

            return default(T);
        }

        private EntityKey GetEntityKey<TEntity>(object keyValue) where TEntity : class
        {
            string entitySetName = GetEntityName<T>();
            ObjectSet<T> objectSet = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<T>();
            string keyPropertyName = objectSet.EntitySet.ElementType.KeyMembers[0].ToString();
            var entityKey = new EntityKey
            (entitySetName, new[] { new EntityKeyMember(keyPropertyName, keyValue) });
            return entityKey;
        }

        private string GetEntityName<TEntity>() where TEntity : class
        {
            // Thanks to Kamyar Paykhan -
            // http://huyrua.wordpress.com/2011/04/13/
            // Entity-framework-4-poco-repository-and-specification-pattern-upgraded-to-ef-4-1/
            // #comment-688
            string entitySetName = ((IObjectContextAdapter)DbContext).ObjectContext
                .MetadataWorkspace
                .GetEntityContainer(((IObjectContextAdapter)DbContext).
                    ObjectContext.DefaultContainerName,
                    DataSpace.CSpace)
                .BaseEntitySets.Where(bes => bes.ElementType.Name == typeof(T).Name).First().Name;
            return string.Format("{0}.{1}",
            ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName,
                entitySetName);
        }

        public IQueryable<T> GetQuery<TEntity>() where TEntity : class
        {
            string EntityName = GetEntityName<T>();
            return ((IObjectContextAdapter)DbContext).ObjectContext.CreateQuery<T>(EntityName);
        }

        public IQueryable<T> GetQuery<TEntity>(Expression<Func<T, bool>> predicate) where TEntity : class
        {
            return GetQuery<T>().Where(predicate);
        }

        public T Single<TEntity>(Expression<Func<T, bool>> criteria) where TEntity : class
        {
            return GetQuery<T>().Single<T>(criteria);
        }

        public void Update(T Entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(Entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(Entity);
            }
            dbEntityEntry.State = EntityState.Modified;

            CancellationToken _CancellationToken = new CancellationToken();
            DbContext.SaveChangesAsync(_CancellationToken);
        }
    }
}
