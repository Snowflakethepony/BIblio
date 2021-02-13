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
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public void CreateReservation(Reservation reservation)
        {
            Create(reservation);
        }

        public void DeleteReservation(Reservation reservation)
        {
            Delete(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationByLibrary(int libraryId)
        {
            return await FindByCondition(r => r.LibraryId == libraryId).Include(r => r.ReservedCopy).Include(r => r.ReservedBy).ToListAsync();
        }

        public async Task<bool> DoesReservationExistForUserByBookCopyId(string userId, int bookCopyId)
        {
            return await FindByCondition(r => r.ReservedCopyId == bookCopyId && r.ReservedById == userId).Select(r => new Reservation
            {
                ReservationId = r.ReservationId
            }).AnyAsync();
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsByUser(string userId)
        {
            return await FindByCondition(r => r.ReservedById == userId).Include(r => r.ReservedCopy).ThenInclude(rc => rc.Book).Include(r => r.Library).ToListAsync();
        }

        public async Task<Reservation> GetReservationById(int ReservationId)
        {
            return await FindByPrimaryKey(ReservationId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBookTitle(string title)
        {
            return await FindByCondition(r => r.ReservedCopy.Book.Title == title).Include(r => r.ReservedCopy).Include(r => r.ReservedBy).ToListAsync();
        }

        public void UpdateReservation(Reservation reservation)
        {
            Update(reservation);
        }

        public async Task<Reservation> GetReservationForUserByBookCipyId(string userId, int bookCopyId)
        {
            return await FindByCondition(r => r.ReservedById == userId && r.ReservedCopyId == bookCopyId).FirstOrDefaultAsync();
        }

        public async Task<Reservation> GetReservationByBookCopyId(int bookCopyId)
        {
            return await FindByCondition(r => r.ReservedCopyId == bookCopyId).FirstOrDefaultAsync();
        }
    }
}
