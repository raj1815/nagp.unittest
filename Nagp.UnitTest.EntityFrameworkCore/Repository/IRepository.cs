using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(object id);
        void Insert(TEntity obj);
        void Update(TEntity obj);
        void Save();
    }
}
