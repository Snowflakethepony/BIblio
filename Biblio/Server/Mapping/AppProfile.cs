using AutoMapper;
using Biblio.Shared.Models;
using Biblio.Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Mapping
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            // ApplicationUser mappings
            CreateMap<AppUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, AppUser>();

            CreateMap<AppUser, ApplicationUserDTOMinusRelations>();
            CreateMap<ApplicationUserDTOMinusRelations, AppUser>();

            // Author mappings
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>();

            // BookCopy mappings
            CreateMap<BookCopy, BookCopyDTO>();
            CreateMap<BookCopyDTO, BookCopy>();

            CreateMap<BookCopy, BookCopyDTOMinusRelations>();
            CreateMap<BookCopyDTOMinusRelations, BookCopy>();

            CreateMap<BookCopyDTO, BookCopyDTOMinusRelations>();
            CreateMap<BookCopyDTOMinusRelations, BookCopyDTO>();

            // Book mappings
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();

            // Genre
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

            // Library mappings
            CreateMap<Library, LibraryDTO>();
            CreateMap<LibraryDTO, Library>();

            CreateMap<Library, LibraryDTOMinusRelations>();
            CreateMap<LibraryDTOMinusRelations, Library>();

            // RentedBookHistory mappings
            CreateMap<BorrowedBookHistory, BorrowedBookHistoryDTO>();
            CreateMap<BorrowedBookHistoryDTO, BorrowedBookHistory>();

            // Reservation mappings
            CreateMap<Reservation, ReservationDTO>();
            CreateMap<ReservationDTO, Reservation>();

            // Application mappings
            CreateMap<Application, ApplicationDTO>();
            CreateMap<ApplicationDTO, Application>();
        }
    }
}
