using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class ApplicationUserDTOMinusRelations
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

        // Library
        public int HomeLibraryId { get; set; }
        public string HomeLibraryName { get; set; }
        public string HomeLibraryAddressLine { get; set; }
        public string HomeLibraryCity { get; set; }
        public string HomeLibraryPostalCode { get; set; }
        public string HomeLibraryPhoneNumber { get; set; }
        public string HomeLibraryEmailAddress { get; set; }
    }
}
