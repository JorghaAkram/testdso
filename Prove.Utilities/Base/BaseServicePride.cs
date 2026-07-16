using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.Utilities.Base
{
    public class BaseServicePride<T> where T : class
    {
        protected DbContext _context;

        public BaseServicePride(DbContext context)
        {
            _context = context;
        }

        public void Add(T obj)
        {
            _context.Set<T>().Add(obj);
            _context.SaveChanges();
        }
        public async Task AddAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync();
        }
        public void AddMany(IEnumerable<T> obj)
        {
            _context.Set<T>().AddRange(obj);
            _context.SaveChanges();
        }
        public async Task AddManyAsync(IEnumerable<T> obj)
        {
            await _context.Set<T>().AddRangeAsync(obj);
            await _context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            _context.Set<T>().Remove(GetById(id));
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            _context.Set<T>().Remove(GetById(id));
            await _context.SaveChangesAsync();
        }

        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(T obj)
        {
            _context.Set<T>().Remove(obj);
            await _context.SaveChangesAsync();
        }
        public void DeleteMany(List<T> obj)
        {
            _context.Set<T>().RemoveRange(obj);
            _context.SaveChanges();
        }
        public async Task DeleteManyAsync(List<T> obj)
        {
            _context.Set<T>().RemoveRange(obj);
            await _context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
        }
        public async Task UpdateAsync(T obj)
        {
            _context.Update(obj);
            await _context.SaveChangesAsync();
        }


        public void UpdateMany(IEnumerable<T> obj)
        {
            _context.UpdateRange(obj);
            _context.SaveChanges();
        }
        public async Task UpdateManyAsync(IEnumerable<T> obj)
        {
            _context.UpdateRange(obj);
            await _context.SaveChangesAsync();
        }
        //public void Update(T obj, int id)
        //{
        //    T data = GetById(id);
        //    if (data != null)
        //    {
        //        _context.Entry(data).CurrentValues.SetValues(obj);
        //        _context.SaveChanges();
        //    }
        //}

        public T UpdateWithReturnObj(T obj, int id)
        {
            if (obj == null)
                return null;

            T data = GetById(id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(obj);
                _context.SaveChanges();
            }

            return data;
        }
        public async Task<T> UpdateWithReturnObjAsync(T obj, int id)
        {
            if (obj == null)
                return null;

            T data = await GetByIdAsync(id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(obj);
                await _context.SaveChangesAsync();
            }

            return data;
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public T FindIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAllIncluding(includeProperties);
            return queryable.FirstOrDefault(match);
        }

        public async Task<T> FindIncludingAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAllIncluding(includeProperties);
            return await queryable.FirstOrDefaultAsync(match);
        }

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().SingleOrDefault(match);
        }
        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        //public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        //{
        //    return _context.Set<T>().Where(match).ToList();
        //}

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        //public IQueryable<T> GetAllDelNo()
        //{
        //    string query = $"select * from {typeof(T).Name} where IsDeleted = 'N'";
        //    return _context.Set<T>().FromSql(query);
        //}

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> match)
        {
            return GetAll().Where(match);
        }

        public IQueryable<T> GetAllIncludingWithFilter(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll().Where(match);
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public int Count()
        {
            return GetAll().Count();
        }
    }
}
