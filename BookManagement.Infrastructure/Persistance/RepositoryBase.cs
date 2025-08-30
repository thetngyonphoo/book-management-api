using BookManagement.Application.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Infrastructure.Persistance
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly AppDbContext _appDbContext;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext ?? throw new ArgumentException(nameof(appDbContext));
            this._dbSet = _appDbContext.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
           await this._dbSet.AddAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await this._dbSet.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = this._dbSet;
            return query.Where(expression);
        }

        public void Remove(T entity)
        {
            this._dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            this._dbSet.Update(entity);
        }
    }
}
