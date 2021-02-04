using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IRepositoryWrapper
    {
        IGenreRepository GenreRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IBookCopyRepository BookCopyRepository { get; }
        IBookRepository BookRepository { get; }
        ILibraryRepository LibraryRepository { get; }
        IBorrowedBookHistoryRepository BorrowedBookHistoryRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IReservationRepository ReservationRepository { get; }
        /// <summary>
        /// Saves the changes made to the context synchronously. 
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        int Save();
        /// <summary>
        /// Saves the changes made to the context asynchronously.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        Task<int> SaveAsync();
    }
}
