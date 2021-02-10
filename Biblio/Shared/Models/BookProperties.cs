using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    public class BookProperties
    {
        public enum BookFormats
        {
            Hardcover,
            Softcover,
            Paperback,
            Folded
        }

        public enum BookTypes
        {
            Fiction,
            NonFiction,
        }
    }
}
