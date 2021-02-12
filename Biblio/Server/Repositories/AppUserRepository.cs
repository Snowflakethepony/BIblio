using Biblio.Server.Data;
using Biblio.Server.Interfaces;
using Biblio.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Repositories
{
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        public AppUserRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext) { }

        public async Task<List<AppUser>> GetAllApplicationUsersByLibrary(int libraryId)
        {
            return await FindByCondition(au => au.HomeLibraryId == libraryId).ToListAsync();
        }

        public async Task<AppUser> GetApplicationUserByBookId(int bookCopyId)
        {
            return await FindByCondition(au => au.BorrowedBooks.Any(bc => bc.BookCopyId == bookCopyId)).FirstOrDefaultAsync();
        }

        public async Task<AppUser> GetApplicationUserById(string id)
        {
            return await FindByPrimaryKey(id);
        }

        public async Task<AppUser> GetApplicationUserByUsername(string username)
        {
            return await FindByCondition(au => au.NormalizedUserName == username.ToUpper()).FirstOrDefaultAsync();
        }

        public void UpdateAppUser(AppUser appUser)
        {
            Update(appUser);
        }
    }
}
