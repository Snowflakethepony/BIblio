using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetAuthorsByPseudonym(string pseudonym);
        Task<Author> GetAuthorByPseudonym(string pseudonym);
        void CreateAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);
    }
}
