using Biblio.Server.Data;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateAuthor(Author author)
        {
            Create(author);
        }

        public void DeleteAuthor(Author author)
        {
            Delete(author);
        }

        public Task<IEnumerable<Author>> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetAuthorById(int id)
        {
            return await FindByPrimaryKey(id);
        }

        public async Task<Author> GetAuthorByPseudonym(string pseudonym)
        {
            return await FindByCondition(a => a.Pseudonym.ToUpper().Contains(pseudonym.ToUpper())).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByPseudonym(string pseudonym)
        {
            var model = typeof(Author);
            return await FindBySqlLike(model.FullName + ", " + model.Assembly.FullName, nameof(Author.Pseudonym), pseudonym).ToListAsync();
        }

        public void UpdateAuthor(Author author)
        {
            Update(author);
        }
    }
}
