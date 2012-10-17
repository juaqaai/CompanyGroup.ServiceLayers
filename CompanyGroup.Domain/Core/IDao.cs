using System.Collections.Generic;

namespace CompanyGroup.Core.DataInterfaces
{
    public interface IDao<T, IdT>
    {
        T GetById(IdT id, bool shouldLock);
        List<T> GetAll();
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void CommitChanges();
    }
}
