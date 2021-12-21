using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nagp.UnitTest.EntityFrameworkCore.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected eTraderDBContext _context = null;
        private DbSet<TEntity> _dbSet = null;

        public GenericRepository(eTraderDBContext _context)
        {
            this._context = _context;
            _dbSet = _context.Set<TEntity>();
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }
        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public void Insert(TEntity obj)
        {
            _dbSet.Add(obj);
        }
        public void Update(TEntity obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
