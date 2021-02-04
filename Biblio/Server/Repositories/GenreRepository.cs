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
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateGenre(Genre genre)
        {
            Create(genre);
        }

        public void DeleteGenre(Genre genre)
        {
            Delete(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Genre> GetGenreById(int id)
        {
            return await FindByPrimaryKey(id);
        }

        public void UpdateGenre(Genre genre)
        {
            Update(genre);
        }
    }
}
