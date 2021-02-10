using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IReservationRepository : IRepositoryBase<Reservation>
    {
        Task<IEnumerable<Reservation>> GetAllReservationsByUser(string userId);
        Task<Reservation> GetReservationById(int ReservationId);
        Task<Reservation> GetReservationForUserByBookCipyId(string userId, int bookCopyId);
        Task<bool> DoesReservationExistForUserByBookCopyId(string userId, int bookCopyId);
        Task<IEnumerable<Reservation>> GetReservationsByBookTitle(string title);
        Task<IEnumerable<Reservation>> GetAllReservationByLibrary(int libraryId);
        void CreateReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
    }
}
