using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Data
{
    public class DatabaseSeeder
    {
        public static async Task SeedDatabase(ApplicationDbContext context)
        {
            // ******************** //
            // CONTRUCTION SECTION  //
            // ******************** //

            // Construct Authors
            var authors = new List<Author>()
            {
                new Author()
                {
                     
                     Firstname = "Stephen",
                     Lastname = "King",
                     FullName = "Stephen King",
                     Pseudonym = "Stephen King",
                     DOB = DateTime.Parse("21-09-1947")
                }
            };

            // Construct Genres
            var genres = new List<Genre>()
            {
                new Genre()
                {
                     Name = "Horror"
                },
                new Genre()
                {
                     Name = "Drama"
                }
            };

            // Construct Books
            var books = new List<Book>()
            {
                new Book()
                {
                     
                    Authors = new List<Author>()
                    {
                        authors.FirstOrDefault(a => a.FullName == "Stephen King")
                    },
                    Title = "IT",
                    Blurb = "'They float... and when you're down here with me, you'll float, too. 'To the children, the town was their whole world. To the adults, knowing better, Derry Maine was just their home town: familiar, well-ordered for the most part. A good place to live. It is the children who see - and feel - what makes the small town of Derry so horribly different. In the storm drains, in the sewers, IT lurks, taking on the shape of every nightmare, each one's deepest dread. Sometimes IT reaches up, seizing, tearing, killing... Time passes and the children grow up, move away and forget. Until they are called back, once more to confront IT as IT stirs and coils in the sullen depths of their memories, reaching up again to make their past nightmares a terrible present reality.",
                    Format = BookProperties.BookFormats.Hardcover,
                    Genres = new List<Genre>()
                    {
                        genres.FirstOrDefault(g => g.Name == "Horror")
                    },
                    NumberofPages = 1392,
                    Weight = 732,
                    Depth = 63,
                    Height = 178,
                    Width = 128,
                    Type = BookProperties.BookTypes.Fiction,
                    PublishedDate = DateTime.Parse("25-07-2017"),
                    Image = null
                }
            };

            // Libraries
            var libraries = new List<Library>()
            {
                new Library()
                {
                    City = "Svenstrup",
                    Name = "Svenstrup Library",
                    PostalCode = "9230",
                    AddressLine = "Godthaebsvej 10",
                    PhoneNumber = "88888888",
                    EmailAddress = "Svenstrupbib@gmail.com"
                }
            };

            context.Genres.AddRange(genres);
            await context.SaveChangesAsync();
            context.Authors.AddRange(authors);
            await context.SaveChangesAsync();
            context.Books.AddRange(books);
            await context.SaveChangesAsync();
            context.Libraries.AddRange(libraries);
            await context.SaveChangesAsync();

            // BookCopies
            var bookCopies = new List<BookCopy>()
            {
                new BookCopy()
                {
                    Book = books.FirstOrDefault(books => books.Title == "IT"),
                    IsAvailable = true,
                    OriginLibrary = libraries.FirstOrDefault(l => l.EmailAddress == "Svenstrupbib@gmail.com"),
                    CurrentLibrary = libraries.FirstOrDefault(l => l.EmailAddress == "Svenstrupbib@gmail.com")       
                }
            };

            // ******************** //
            // CONTEXT SECTIOIN     //
            // ******************** //

            context.BookCopies.AddRange(bookCopies);

            // SAVING TO DATABASE!
            await context.SaveChangesAsync();
        }
    }
}
