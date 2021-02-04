using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
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
        public string Pseudonym { get; set; } = "";
    }
}
