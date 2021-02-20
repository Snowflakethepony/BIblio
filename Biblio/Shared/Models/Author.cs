using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
        public string Pseudonym { get; set; } = "";
        public DateTime? DOB { get; set; }

        // Relations
        public ICollection<Book> Books { get; set; }
    }
}
