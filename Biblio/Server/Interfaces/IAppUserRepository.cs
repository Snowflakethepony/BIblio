using Biblio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Interfaces
{
    public interface IAppUserRepository : IRepositoryBase<ApplicationUser>
    {
        Task<List<ApplicationUser>> GetAllApplicationUsersByLibrary(int libraryId);
        Task<ApplicationUser> GetApplicationUserById(string id);
        Task<ApplicationUser> GetApplicationUserByBookId(int bookCopyId);
    }
}
