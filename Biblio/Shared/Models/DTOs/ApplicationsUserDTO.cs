using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class ApplicationsUserDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        public DateTime? DOB { get; set; }

        // Relations
        public int HomeLibraryId { get; set; }
        public LibraryDTOMinusRelations HomeLibrary { get; set; }

        public ICollection<BookCopyDTO> BorrowedBooks { get; set; }
        public ICollection<BorrowedBookHistoryDTO> BorrowedBookHistories { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
