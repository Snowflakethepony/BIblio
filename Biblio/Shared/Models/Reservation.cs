using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        public DateTime ReservedAt { get; set; }
        public bool IsSent { get; set; }
        public bool IsForeignBorrower { get; set; }
        public DateTime? ExpirationDate { get; set; }

        // Relations
        [ForeignKey("BookCopyId")]
        public int ReservedCopyId { get; set; }
        public BookCopy ReservedCopy { get; set; }

        [ForeignKey("LibraryId")]
        public int LibraryId { get; set; }
        public Library Library { get; set; }

        [ForeignKey("ApplicationUserId")]
        public string ReservedById { get; set; }
        public AppUser ReservedBy { get; set; }
    }
}
