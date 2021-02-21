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
    public class BorrowedBookHistoryRepository : RepositoryBase<BorrowedBookHistory>, IBorrowedBookHistoryRepository
    {
        public BorrowedBookHistoryRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateRentedBookHistory(BorrowedBookHistory rentedBookHistory)
        {
            Create(rentedBookHistory);
        }

        public void DeleteRentedBookHistory(BorrowedBookHistory rentedBookHistory)
        {
            Delete(rentedBookHistory);
        }

        public async Task<IEnumerable<BorrowedBookHistory>> GetAllRentedBookHistoriesForUser(string userId)
        {
            return await FindByCondition(rbh => rbh.BorrowerId == userId).Include(rbh => rbh.Book).ThenInclude(bc => bc.Book).Include(rbh => rbh.Borrower).ToListAsync();
        }

        public async Task<IEnumerable<BorrowedBookHistory>> GetAllRentedBookHistoriesForUserByBookId(string userId, int bookId)
        {
            return await FindByCondition(rbh => rbh.BorrowerId == userId && rbh.BookCopyId == bookId).Include(rbh => rbh.Book).Include(rbh => rbh.Borrower).ToListAsync();
        }

        public async Task<BorrowedBookHistory> GetRentedBookHistoryById(int rentedBookHistoryId)
        {
            return await FindByPrimaryKey(rentedBookHistoryId);
        }

        public async Task<BorrowedBookHistory> GetRentedBookHistoryByIdIncludingRelations(int rentedBookHistoryId)
        {
            return await FindByCondition(rbh => rbh.BorrowedBookHistoryId == rentedBookHistoryId).Include(rbh => rbh.Book).Include(rbh => rbh.Borrower).SingleOrDefaultAsync();
        }

        public void UpdateRentedBookHistory(BorrowedBookHistory rentedBookHistory)
        {
            Update(rentedBookHistory);
        }
    }
}
