using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prove.Utilities.Base
{
    public interface IBaseCrud<T>
    {

        void Add(T obj);
        Task AddAsync(T obj);
        void AddMany(IEnumerable<T> obj);
        Task AddManyAsync(IEnumerable<T> obj);
        void Delete(int id);
        Task DeleteAsync(int id);
        void Delete(T obj);
        Task DeleteAsync(T obj);

        void DeleteMany(List<T> obj);
        Task DeleteManyAsync(List<T> obj);


        //void Update(T obj, int id);
        void Update(T obj);
        Task UpdateAsync(T obj);
        void UpdateMany(IEnumerable<T> obj);
        Task UpdateManyAsync(IEnumerable<T> obj);
        T UpdateWithReturnObj(T obj, int id);
        Task<T> UpdateWithReturnObjAsync(T obj, int id);
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        T FindIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        Task<T> FindIncludingAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        T Find(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        //ICollection<T> FindAll(Expression<Func<T, bool>> match);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> match);
        IQueryable<T> GetAllIncludingWithFilter(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        int Count();
        //void Add(T obj);
        //void AddMany(List<T> obj);
        //void Delete(int id);
        //void Delete(T obj);


        ////void Update(T obj, int id);
        //void Update(T obj);
        //void UpdateMany(IEnumerable<T> obj);
        //T UpdateWithReturnObj(T obj, int id);
        //T GetById(int id);
        //T FindIncluding(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        //T Find(Expression<Func<T, bool>> match);
        ////ICollection<T> FindAll(Expression<Func<T, bool>> match);
        //IQueryable<T> GetAll();
        //IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        //IQueryable<T> GetAllWithFilter(Expression<Func<T, bool>> match);
        //IQueryable<T> GetAllIncludingWithFilter(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
        //int Count();
    }
}
