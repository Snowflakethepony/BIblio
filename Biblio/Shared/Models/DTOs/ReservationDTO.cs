using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public DateTime ReservedAt { get; set; }
        public bool IsSent { get; set; }
        public bool IsForeignBorrower { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Relations
        public int ReservedCopyId { get; set; }
        public BookCopyDTO ReservedCopy { get; set; }

        public int LibraryId { get; set; }
        public LibraryDTOMinusRelations Library { get; set; }

        public string ReservedById { get; set; }
        public ApplicationUserDTOMinusRelations ReservedBy { get; set; }
    }
}
