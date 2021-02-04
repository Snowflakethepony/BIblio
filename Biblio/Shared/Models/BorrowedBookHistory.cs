using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class BorrowedBookHistory
    {
        public int BorrowedBookHistoryId { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime ReturnedAt { get; set; }
        public int TimesRerented { get; set; }

        // Relations
        [ForeignKey("Id")]
        public string BorrowerId { get; set; }
        public ApplicationUser Borrower { get; set; }

        [ForeignKey("BookCopyId")]
        public int BookCopyId { get; set; }
        public BookCopy Book { get; set; }
    }
}
