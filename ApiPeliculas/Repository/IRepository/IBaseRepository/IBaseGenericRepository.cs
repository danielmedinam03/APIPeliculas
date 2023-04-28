using ApiPeliculas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ApiPeliculas.Repository.IRepository.IBaseRepository
{
    public interface IBaseGenericRepository<T> where T : class
    {
        Context _context { get; set; }
        DbSet<T> Entity => _context.Set<T>();

        public Task<U> GenericGetAsync<U>(Expression<Func<T, U>> parameter,
                                          Expression<Func<T, bool>> searchQuery = null,
                                          Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public Task<List<T>> GenericGetAllAsync(Expression<Func<T, bool>> predicate = null,
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                Expression<Func<T, T>> selector = null,
                                                bool distinct = false);
        public Task<bool> ExistGenericAsync(Expression<Func<T, bool>> predicate = null,
                                            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        public Task<bool> UpdateGenericAsync(T value, bool saveChanges = true,
                                            params Expression<Func<T, object>>[] propertyExpresion);
        public Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate = null, bool SaveChanges = true,
                                        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        public Task<bool> AddAsync(T entity, bool SaveChanges = true);
        public Task<int> SaveChangesAsync();

    }
}
