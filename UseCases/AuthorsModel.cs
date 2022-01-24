using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Model
{
    public class AuthorsModel : ICrud<IAuthor>, IPagination<IAuthor>
    {
        AuthorsRepository Authors { get; init; } = new PostgreSqlFactory().GetAuthorsRepository();

        public void Add(IAuthor entity)
        {
            Authors.Add(entity);
        }

        public void Delete(int id)
        {
            Authors.Delete(id);
        }

        public IAuthor Get(int id)
        {
            return Authors.Get(id);
        }

        public IAuthor[] Get(int page, int pageSize)
        {
            return Authors.Get(page, pageSize);
        }

        public void Update(IAuthor entity)
        {
            Authors.Update(entity);
        }
    }
}
