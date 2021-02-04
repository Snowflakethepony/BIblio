using Biblio.Server.Data;
using Biblio.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        protected ApplicationDbContext DbContext { get; set; }
        private IGenreRepository genreRepository;
        private IAuthorRepository authorRepository;
        private IBookCopyRepository bookCopyRepository;
        private IBookRepository bookRepository;
        private ILibraryRepository libraryRepository;
        private IBorrowedBookHistoryRepository borrowedBookHistoryRepository;
        private IAppUserRepository appUserRepository;
        private IReservationRepository reservationRepository;

        public RepositoryWrapper(ApplicationDbContext applicationDbContext)
        {
            this.DbContext = applicationDbContext;
        }

        public IGenreRepository GenreRepository
        {
            get
            {
                if (genreRepository == null)
                {
                    genreRepository = new GenreRepository(DbContext);
                }

                return genreRepository;
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                if (authorRepository == null)
                {
                    authorRepository = new AuthorRepository(DbContext);
                }

                return authorRepository;
            }
        }

        public IBookCopyRepository BookCopyRepository
        {
            get
            {
                if (bookCopyRepository == null)
                {
                    bookCopyRepository = new BookCopyRepository(DbContext);
                }

                return bookCopyRepository;
            }
        }

        public IBookRepository BookRepository
        {
            get
            {
                if (bookRepository == null)
                {
                    bookRepository = new BookRepository(DbContext);
                }

                return bookRepository;
            }
        }

        public ILibraryRepository LibraryRepository
        {
            get
            {
                if (libraryRepository == null)
                {
                    libraryRepository = new LibraryRepository(DbContext);
                }

                return libraryRepository;
            }
        }

        public IBorrowedBookHistoryRepository BorrowedBookHistoryRepository
        {
            get
            {
                if (borrowedBookHistoryRepository == null)
                {
                    borrowedBookHistoryRepository = new BorrowedBookHistoryRepository(DbContext);
                }

                return borrowedBookHistoryRepository;
            }
        }

        public IAppUserRepository AppUserRepository
        {
            get
            {
                if (appUserRepository == null)
                {
                    appUserRepository = new AppUserRepository(DbContext);
                }

                return appUserRepository;
            }
        }

        public IReservationRepository ReservationRepository
        {
            get
            {
                if (reservationRepository == null)
                {
                    reservationRepository = new ReservationRepository(DbContext);
                }

                return reservationRepository;
            }
        }

        public int Save()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
