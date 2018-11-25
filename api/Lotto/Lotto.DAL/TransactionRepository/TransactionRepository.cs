using Lotto.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lotto.DAL.TransactionRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;
        private IDbContextTransaction _currentScope;
        private readonly string _userId;

        public TransactionRepository(DataContext context, IHttpContextAccessor contextAccesspr)
        {
            _context = context;
            _userId = contextAccesspr.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

        public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            var query = Set<T>().AsNoTracking().AsQueryable();
            query = DomainFilter(query);

            return where == null ? query : query.Where(where);
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                Rollback();
            }
            return 0;
        }

        public void Attach<T>(T item) where T : class
        {
            Set<T>().Attach(item);
        }

        public IDisposable Begin()
        {
            _currentScope = _context.Database.BeginTransaction();
            return _currentScope;
        }
        public void Rollback()
        {
            if (_currentScope == null) return;

            _currentScope.Rollback();
            _currentScope.Dispose();
            _currentScope = null;
        }
        public void End()
        {
            if (_currentScope == null)
                throw new Exception("Missing Transaction");
            _currentScope.Commit();
            _currentScope = null;
        }

        public void Dispose()
        {
            Rollback();
            _context.Dispose();
        }

        public int Update<T>(T entityAfterUpdate, T entityBeforeUpdate, params string[] propertyName)
            where T : class
        {
            Set<T>().Attach(entityAfterUpdate);
            var entityEntry = _context.Entry(entityAfterUpdate);

            foreach (var name in propertyName)
                entityEntry.Property(name).IsModified = true;

            var result = Save();
            entityEntry.State = EntityState.Detached;
            return result;
        }

        public int Update<T>(T entity) where T : class
        {
            Set<T>().Attach(entity);
            var entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;

            var result = Save();
            entityEntry.State = EntityState.Detached;
            return result;
        }

        public int Add<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
            var res = Save();
            var entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Detached;
            return res;
        }

        public int Remove<T>(T toRemove) where T : class
        {
            Set<T>().Attach(toRemove);
            Set<T>().Remove(toRemove);

            return Save();
        }

        protected virtual IQueryable<T> DomainFilter<T>(IQueryable<T> query) => query;

        private DbSet<T> Set<T>() where T : class => _context.Set<T>();
    }
}
