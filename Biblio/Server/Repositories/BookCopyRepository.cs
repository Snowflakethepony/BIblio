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
    public class BookCopyRepository : RepositoryBase<BookCopy>, IBookCopyRepository
    {
        public BookCopyRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateBookCopy(BookCopy bookCopy)
        {
            Create(bookCopy);
        }

        public void DeleteBookCopy(BookCopy bookCopy)
        {
            Delete(bookCopy);
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookId(int bookId)
        {
            return await FindByCondition(bc => bc.BookId == bookId).Include(bc => bc.Borrower).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).Include(bc => bc.Book).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookIdForLibrary(int bookId, int libraryId)
        {
            return await FindByCondition(bc => bc.BookId == bookId && bc.OriginLibraryId == libraryId).Include(bc => bc.Borrower).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesForLibrary(int libraryId)
        {
            return await FindByCondition(bc => bc.OriginLibraryId == libraryId).Include(bc => bc.Borrower).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesForUser(string userId)
        {
            return await FindByCondition(bc => bc.BorrowerId == userId).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesByBookTitle(string title)
        {
            return await (from bc in base.DbContext.BookCopies.Include(bc => bc.OriginLibrary).Include(bc => bc.Book).ThenInclude(b => b.Genres).AsNoTracking()
                          where bc.Book.Title.Contains(title)
                          select bc).ToListAsync();

            //return await FindBySqlLike(typeof(BookCopy), ).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesByAuthor(string author)
        {
            //return from bc in base.DbContext.BookCopies.Include(bc => bc.OriginLibrary).Include(bc => bc.Book)
            //       where exists 
            //       select bc;

            //return from bc in base.DbContext.BookCopies.Include(bc => bc.OriginLibrary).Include(bc => bc.Book)
            //       where
            //       select bc;
            return await base.DbContext.BookCopies.FromSqlRaw($@"SELECT * FROM BookCopies 
                INNER JOIN Books B on B.BookId = bc.BookId
                INNER JOIN Libraries L on L.LibraryId = bc.OriginLibraryId
                WHERE bc.BookId = (SELECT b.BookId from Books as b
                    INNER JOIN AuthorBook AB on b.BookId = AB.BooksBookId
                    INNER JOIN Authors A on A.AuthorId = AB.AuthorsAuthorId
                    WHERE A.Pseudonym LIKE '{author}%')").ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesByGenre(string genre)
        {
            //return from bc in base.DbContext.BookCopies.Include(bc => bc.OriginLibrary).Include(bc => bc.Book)
            //       where bc.Book.Genres.Contains(genre)
            //       select bc;

            //return await base.DbContext.BookCopies.FromSqlRaw($@"SELECT * FROM BookCopies
            //    WHERE BookId = (SELECT b.BookId from Books as b
            //        INNER JOIN BookGenre BG on b.BookId = BG.BooksBookId
            //        inner join Genres G on G.GenreId = BG.GenresGenreId
            //        WHERE g.Name like '{genre}%')").ToListAsync();

            try
            {

                //var bookCopies = (from bc in base.DbContext.BookCopies.Include(bc => bc.Book).ThenInclude(b => b.Authors).Include(bc => bc.OriginLibrary).AsNoTracking()
                //                  where bc.BookId == 1
                //                 ).ToListAsync();


                // bc.BookCopyId, bc.BookId, bc.OriginLibraryId, bc.CurrentLibraryId, bc.IsAvailable, bc.ShelfNumber, bc.BorrowedAt, bc.ReturnBy, bc.TimesRerented, bc.BorrowerId, bc.RFID, b.Title, b.Blurb, b.PublishedDate, b.Height, b.Depth, b.Width, b.Weight, b.NumberofPages, b.Image, b.Format, b.Type, l.Name, l.AddressLine, l.City, l.PostalCode, l.PhoneNumber, l.EmailAddress, a.Firstname, a.Lastname, a.Pseudonym, a.DOB
                return await base.DbContext.BookCopies.FromSqlRaw($@"SELECT * FROM BookCopies as bc
                    INNER JOIN Books B on B.BookId = bc.BookId
                    INNER JOIN Libraries L on L.LibraryId = bc.OriginLibraryId
                    INNER JOIN AuthorBook AB on B.BookId = AB.BooksBookId
                    INNER JOIN Authors A on A.AuthorId = AB.AuthorsAuthorId
                    WHERE bc.BookId = (SELECT BA.BookId from Books as BA
                        INNER JOIN BookGenre BG on BA.BookId = BG.BooksBookId
                        inner join Genres G on G.GenreId = BG.GenresGenreId
                        WHERE G.Name like '{ genre }%')").AsNoTracking().ToListAsync();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //return await FindByCondition(bc => bc.BorrowerId == userId).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<IEnumerable<BookCopy>> GetAllBookCopiesReturnOverdueForLibrary(int libraryId)
        {
            return await FindByCondition(bc => bc.OriginLibraryId == libraryId && bc.ReturnBy < DateTime.Now).Include(bc => bc.Borrower).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).ToListAsync();
        }

        public async Task<BookCopy> GetBookCopyById(int bookCopyId)
        {
            return await FindByPrimaryKey(bookCopyId);
        }

        public async Task<BookCopy> GetBookCopyByIdIncludingRelation(int bookCopyId)
        {
            return await FindByCondition(bc => bc.BookCopyId == bookCopyId).Include(bc => bc.Borrower).Include(bc => bc.OriginLibrary).Include(bc => bc.CurrentLibrary).SingleOrDefaultAsync();
        }

        public void UpdateBookCopy(BookCopy bookCopy)
        {
            Update(bookCopy);
        }
    }
}
