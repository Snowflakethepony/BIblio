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
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();

            CreateMap<ApplicationUser, ApplicationUserDTOMinusRelations>();
            CreateMap<ApplicationUserDTOMinusRelations, ApplicationUser>();

            // Author mappings
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>();

            // BookCopy mappings
            CreateMap<BookCopy, BookCopyDTO>();
            CreateMap<BookCopyDTO, BookCopy>();

            // Book mappings
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();

            // Genre
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

            // Library mappings
            CreateMap<Shared.Models.Library, LibraryDTO>();
            CreateMap<LibraryDTO, Shared.Models.Library>();

            CreateMap<Shared.Models.Library, LibraryDTOMinusRelations>();
            CreateMap<LibraryDTOMinusRelations, Shared.Models.Library>();

            // RentedBookHistory mappings
            CreateMap<BorrowedBookHistory, BorrowedBookHistoryDTO>();
            CreateMap<BorrowedBookHistoryDTO, BorrowedBookHistory>();
        }
    }
}
