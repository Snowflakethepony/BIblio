using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models
{
    /// <summary>
    /// This model is for a shared understanding of what a book is. A book is unique with format incl. this means that multiple physical books still refers to the same instance of this model/ row in the database. 
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Booksformats supported.
        /// </summary>
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

        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Blurb { get; set; }
        public DateTime PublishedDate { get; set; }
        public short? Height { get; set; }
        public short? Depth { get; set; }
        public short? Width { get; set; }
        public short? Weight { get; set; }
        public short NumberofPages { get; set; }
        public byte[]? Image { get; set; }
        public BookFormats Format { get; set; }
        public BookTypes Type { get; set; }

        // Relations
        public ICollection<Author> Authors { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
