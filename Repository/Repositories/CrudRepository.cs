using Microsoft.EntityFrameworkCore;
using Model;
using Repository.DTO;

namespace Repository
{
    internal class CrudRepository<T> : Repository, ICrud<T> where T : class, IRecordId
    {
        public CrudRepository(Func<BooksContext> getContext) : base(getContext)
        {
        }

        public void Add(T entity)
        {
            try
            {
                if (entity is object)
                {
                    using var context = GetContext();
                    context.Add(entity);
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException e)
            {
                throw new Model.Exceptions.InvalidEntityException("Entity with this id cannot be added", e);
            }
        }

        public void Delete(int id)
        {
            using var context = GetContext();
            var entities = context.Set<T>();
            try
            {
                var entity = entities.Single(n => n.Id == id);
                entities.Remove(entity);
                context.SaveChanges();
            }
            catch(InvalidOperationException e)
            {
                throw new Model.Exceptions.EntityNotFoundException("Entity with this id was not found", e);
            }
        }

        public T Get(int id)
        {
            using var context = GetContext();
            var entities = context.Set<T>();
            var result = entities.Find(id);
            if (result is object)
                return result;
            else
                throw new Model.Exceptions.EntityNotFoundException($"Entity with this id was not found");
        }

        public void Update(T entity)
        {
            try
            {
                if (entity is object)
                {
                    using var context = GetContext();
                    var entities = context.Set<T>();
                    entities.Update(entity);
                    context.SaveChanges();
                }
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new Model.Exceptions.InvalidEntityException("Entity with this id is missing", e);
            }
        }
    }
}
