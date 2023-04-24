using Microsoft.EntityFrameworkCore;
using TriviaAPI.Models;

namespace TriviaAPI.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly Sistem21TriviaContext context;

        public Repository(Sistem21TriviaContext context)
        {
            this.context = context;
        }

        public IEnumerable<T> GetAll() 
        {
            return context.Set<T>();
        }

        public T? GetbyId(object id)
        {
            return context.Find<T>(id);
        }

        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }

    }
}
