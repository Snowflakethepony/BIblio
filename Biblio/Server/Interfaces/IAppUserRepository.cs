using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IAppUserRepository : IRepositoryBase<AppUser>
    {
        Task<List<AppUser>> GetAllApplicationUsersByLibrary(int libraryId);
        Task<AppUser> GetApplicationUserById(string id);
        Task<AppUser> GetApplicationUserByBookId(int bookCopyId);
        Task<AppUser> GetApplicationUserByUsername(string username);
        void UpdateAppUser(AppUser appUser);
    }
}
