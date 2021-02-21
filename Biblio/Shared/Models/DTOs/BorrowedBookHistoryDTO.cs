using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class BorrowedBookHistoryDTO
    {
        public int BorrowedBookHistoryId { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime ReturnedAt { get; set; }
        public int TimesRerented { get; set; }

        // Relations
        public int BookCopyId { get; set; }
        public BookCopyDTO Book { get; set; }
    }
}
