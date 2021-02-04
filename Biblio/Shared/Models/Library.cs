using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class Library
    {
        [Key]
        public int LibraryId { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        // Relations
        public ICollection<BookCopy> OwnedBooks { get; set; }
        public ICollection<BookCopy> ForeignBooks { get; set; }
    }
}
