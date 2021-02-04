using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class BookCopyDTO
    {
        public int BookCopyId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? RentedAt { get; set; }
        public DateTime? ReturnBy { get; set; }
        public int? TimesRerented { get; set; }

        /// <summary>
        /// JUST HERE TO IMMULATE A REAL SCAN PROPERTY ON PHYSICAL BOOKS! 
        /// Used in system to loan, return and rerent by scanning and locating the book instance inside the system. 
        /// This is just randomly generated at BookCopy instance creation to an 8 letter string.
        /// </summary>
        public string RFID { get; private set; }


        // Relations
        public int BookId { get; set; }
        public BookDTO Book { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUserDTOMinusRelations Renter { get; set; }

        public int OriginalLibraryId { get; set; }
        public LibraryDTOMinusRelations OriginLibrary { get; set; }

        public int CurrentLibraryId { get; set; }
        public LibraryDTOMinusRelations CurrentLibrary { get; set; }
    }
}
