using System;
using System.Linq;
using System.Linq.Expressions;

namespace Lotto.DAL.TransactionRepository
{
    public interface ITransactionRepository : IDisposable
    {
        IQueryable<T> GetAll<T>(Expression<Func<T, bool>> where = null) where T : class;
        int Save();
        int Update<T>(T entityAfterUpdate, T entityBeforeUpdate, params string[] propertyName) where T : class;
        int Update<T>(T entity) where T : class;
        void Attach<T>(T item) where T : class;
        int Add<T>(T entity) where T : class;
        IDisposable Begin();
        void End();
        void Rollback();
        int Remove<T>(T toRemove) where T : class;
    }
}