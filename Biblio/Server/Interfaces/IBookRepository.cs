using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<Book>> GetBooksByAuthor(int authorId);
        Task<IEnumerable<Book>> GetBooksByGenre(int genreId);
        Task<Book> GetBookById(int bookId);
        Task<Book> GetBookByIdIncludingRelations(int bookId);
        Task<Book> GetBookByTitle(string title);
        Task<IEnumerable<Book>> GetBooksByTitleAndFormat(string title, BookProperties.BookFormats format);
        Task<IEnumerable<Book>> SearchBooksByTitle(string title);
        void CreateBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(Book book);
    }
}
