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
