using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class BookCopy
    {
        // Static property that states how many times the same renter can rent the copy in a ROW.
        public static int MaxRerents = 3;

        [Key]
        public int BookCopyId { get; set; }
        public bool IsAvailable { get; set; }
        public int ShelfNumber { get; set; }
        public DateTime? BorrowedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public int? TimesRerented { get; set; }

        /// <summary>
        /// JUST HERE TO IMMULATE A REAL SCAN PROPERTY ON PHYSICAL BOOKS! 
        /// Used in system to loan, return and rerent by scanning and locating the book instance inside the system. 
        /// This is just randomly generated at BookCopy instance creation to an 8 letter string.
        /// </summary>
        public string RFID { get; set; }

        // Relations
        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("Id")]
        public string BorrowerId { get; set; }
        public AppUser Borrower { get; set; }

        [ForeignKey("LibraryId")]
        public int OriginLibraryId { get; set; }
        public Library OriginLibrary { get; set; }

        [ForeignKey("LibraryId")]
        public int CurrentLibraryId { get; set; }
        public Library CurrentLibrary { get; set; }

        public BookCopy()
        {
            RFID = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
        }
    }
}
