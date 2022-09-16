using GitChat.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GitChat.Server.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(T item)
        {
            context.Set<T>().Add(item);
        }

        public void Delete(T item)
        {
            context.Set<T>().Remove(item);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void SetValues(T OldItem, T NewItem)
        {
            context.Entry(OldItem).CurrentValues.SetValues(NewItem);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

    }
}
