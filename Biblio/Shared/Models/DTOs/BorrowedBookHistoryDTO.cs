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
        public DateTime BorrowerdAt { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public string ApplicationUserId { get; set; }
        public string ApplicationUserFirstname { get; set; }
        public string ApplicationUserLastname { get; set; }
        public string ApplicationUserFullname { get; set; }
    }
}
