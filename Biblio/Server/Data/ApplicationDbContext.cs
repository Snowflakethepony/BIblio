using Biblio.Shared.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BorrowedBookHistory> BorrowedBookHistories { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            base.OnModelCreating(modelBuilder);

            // THIS SETS THE DEFAULT FOR ALL RELATIONSHIPS ON A SINGLE MODEL!!!!
            foreach (var relationship in modelBuilder.Model.FindEntityType("Biblio.Shared.Models.Reservation").GetForeignKeys())
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // THIS SETS THE "DEFAULT" DELETE BEHAVIOR ON ALL RELATIONSHIPS!!!!
            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().GetForeignKeys())
            //{
            //    relationship.DeleteBehavior = DeleteBehavior.NoAction;
            //}

            //modelBuilder.Entity("Library.Shared.Models.Reservation", b =>
            //{
            //    b.HasOne("Library.Shared.Models.Library")
            //        .WithMany()
            //        .HasForeignKey("LibraryId")
            //        .OnDelete(DeleteBehavior.NoAction);
            //});

            // ********** //
            // Below logic is meant to deal with 2 one-to-many relations between BookCopy and Library. //
            // One with cascade delete as that library owns the instance of the bookcopy the other with no action as that libray instance are only borrowing the instance of bookcopy //
            // ********** //
            modelBuilder.Entity<BookCopy>()
                .HasOne(bc => bc.CurrentLibrary)
                .WithMany(l => l.ForeignBooks)
                .HasForeignKey(bc => bc.CurrentLibraryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookCopy>()
                .HasOne(bc => bc.OriginLibrary)
                .WithMany(l => l.OwnedBooks)
                .HasForeignKey(bc => bc.OriginLibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Reservation>()
            //    .HasOne(bc => bc.Library)
            //    .WithMany(l => l.)
            //    .HasForeignKey(bc => bc.OriginLibraryId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Custom table names.
            modelBuilder.Entity<Book>().ToTable("Books");
        }
    }
}
