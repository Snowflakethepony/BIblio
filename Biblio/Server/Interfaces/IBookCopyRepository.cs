using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IBookCopyRepository : IRepositoryBase<BookCopy>
    {
        Task<IEnumerable<BookCopy>> GetAllBookCopiesForLibrary(int libraryId);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesReturnOverdueForLibrary(int libraryId);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookIdForLibrary(int bookId, int libraryId);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookId(int bookId);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookTitle(string title);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesByAuthor(string author);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesByGenre(string genre);
        Task<BookCopy> GetBookCopyByRFID(string RFID);
        Task<IEnumerable<BookCopy>> GetAllBookCopiesForUser(string userId);
        Task<BookCopy> GetBookCopyById(int bookCopyId);
        Task<BookCopy> GetBookCopyByIdIncludingRelation(int bookCopyId);
        void CreateBookCopy(BookCopy bookCopy);
        void UpdateBookCopy(BookCopy bookCopy);
        void DeleteBookCopy(BookCopy bookCopy);
    }
}
