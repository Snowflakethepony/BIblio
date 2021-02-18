using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        /// <summary>
        /// Tells if the application has been handled by an admin.
        /// </summary>
        public bool IsHandled { get; set; } = false;
        /// <summary>
        /// Tells if the application was verified by an admin. Non-verified application are not applicable for creation of a library.
        /// </summary>
        public bool IsVerified { get; set; } = false;
    }
}
