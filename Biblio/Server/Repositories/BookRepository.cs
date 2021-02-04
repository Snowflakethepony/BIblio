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
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateBook(Book book)
        {
            Create(book);
        }

        public void DeleteBook(Book book)
        {
            Delete(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await FindAll().Include(b => b.Authors).Include(b => b.Genres).ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await FindByPrimaryKey(id);
        }

        public async Task<Book> GetBookByIdIncludingRelations(int bookId)
        {
            return await FindByCondition(b => b.BookId == bookId).Include(b => b.Authors).Include(b => b.Genres).SingleOrDefaultAsync();
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            return await FindByCondition(b => b.Title.ToLower().Contains(title.ToLower())).Include(b => b.Authors).Include(b => b.Genres).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthor(int authorId)
        {
            return await FindByCondition(b => b.Authors.Contains(b.Authors.SingleOrDefault(a => a.AuthorId == authorId))).Include(b => b.Genres).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenre(int genreId)
        {
            return await FindByCondition(b => b.Genres.Contains(b.Genres.SingleOrDefault(g => g.GenreId == genreId))).Include(b => b.Authors).Include(b => b.Genres).ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksByTitle(string title)
        {
            return await FindBySqlLike(nameof(Book), nameof(Book.Title), title).Include(b => b.Authors).Include(b => b.Genres).ToListAsync();
        }

        public void UpdateBook(Book book)
        {
            Update(book);
        }
    }
}
