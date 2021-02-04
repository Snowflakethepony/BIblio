using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IBorrowedBookHistoryRepository : IRepositoryBase<BorrowedBookHistory>
    {
        Task<IEnumerable<BorrowedBookHistory>> GetAllRentedBookHistoriesForUser(string userId);
        Task<IEnumerable<BorrowedBookHistory>> GetAllRentedBookHistoriesForUserByBookId(string userId, int bookId);
        Task<BorrowedBookHistory> GetRentedBookHistoryById(int rentedBookHistoryId);
        Task<BorrowedBookHistory> GetRentedBookHistoryByIdIncludingRelations(int rentedBookHistoryId);
        void CreateRentedBookHistory(BorrowedBookHistory rentedBookHistory);
        void UpdateRentedBookHistory(BorrowedBookHistory rentedBookHistory);
        void DeleteRentedBookHistory(BorrowedBookHistory rentedBookHistory);
    }
}
