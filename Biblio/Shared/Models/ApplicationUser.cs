using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Biblio.Shared.Models
{
    public class ApplicationUser : IdentityUser
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
        [ForeignKey("LibraryId")]
        public int? HomeLibraryId { get; set; }
        public Library HomeLibrary { get; set; }

        public ICollection<BookCopy> BorrowedBooks { get; set; }
        public ICollection<BorrowedBookHistory> BorrowedBookHistories { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
