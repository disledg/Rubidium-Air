using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubidium
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IQueryable<T> GetAll() => _dbSet.AsQueryable();

        public virtual T GetById(int id) => _dbSet.Find(id);

        public virtual void Add(T entity) => _dbSet.Add(entity);

        public virtual void Update(T entity)
        {
            var existing = _dbSet.Find(entity.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
            }
            Console.WriteLine($"Updating entity ID: {entity.Id}");
        }

        public virtual void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public virtual void Save() => _context.SaveChanges();
    }
}
