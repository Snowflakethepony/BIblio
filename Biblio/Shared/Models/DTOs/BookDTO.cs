using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Shared.Models.DTOs
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public DateTime PublishedDate { get; set; }
        public short? Height { get; set; }
        public short? Depth { get; set; }
        public short? Width { get; set; }
        public short? Weight { get; set; }
        public int NumberofPages { get; set; }
        public byte[] Image { get; set; }
        public BookProperties.BookFormats Format { get; set; }
        public BookProperties.BookTypes Type { get; set; }

        public ICollection<AuthorDTO> Authors { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
    }
}
